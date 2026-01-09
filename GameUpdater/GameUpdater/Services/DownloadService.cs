using System.Diagnostics;
using System.IO;
using System.Net.Http;
using GameUpdater.Models;
using GameUpdater.Services.Interfaces;

namespace GameUpdater.Services;

/// <summary>
/// Service for downloading files with progress reporting
/// </summary>
public class DownloadService : IDownloadService, IDisposable
{
    private readonly HttpClient _httpClient;
    private CancellationTokenSource? _cancellationTokenSource;
    private string _currentFileName = string.Empty;
    private int _currentFileIndex;
    private int _totalFiles;

    public event EventHandler<DownloadProgress>? ProgressChanged;

    public DownloadService()
    {
        _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromMinutes(30)
        };
    }

    public async Task<string> DownloadStringAsync(string url, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _httpClient.GetStringAsync(url, cancellationToken);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error downloading string from {url}: {ex.Message}");
            throw;
        }
    }

    public void SetCurrentFileInfo(string fileName, int currentIndex, int totalFiles)
    {
        _currentFileName = fileName;
        _currentFileIndex = currentIndex;
        _totalFiles = totalFiles;
    }

    public async Task DownloadFileAsync(string url, string localPath, long expectedSize, CancellationToken cancellationToken = default)
    {
        // Ensure directory exists
        var directory = Path.GetDirectoryName(localPath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();

        var totalBytes = response.Content.Headers.ContentLength ?? expectedSize;

        using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var fileStream = new FileStream(localPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);

        var buffer = new byte[8192];
        long totalBytesRead = 0;
        var stopwatch = Stopwatch.StartNew();
        int bytesRead;
        var lastProgressUpdate = DateTime.MinValue;

        while ((bytesRead = await contentStream.ReadAsync(buffer, cancellationToken)) > 0)
        {
            await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken);
            totalBytesRead += bytesRead;

            // Update progress every 100ms to reduce UI updates
            if ((DateTime.Now - lastProgressUpdate).TotalMilliseconds >= 100)
            {
                var elapsed = stopwatch.Elapsed.TotalSeconds;
                var speed = elapsed > 0 ? totalBytesRead / elapsed : 0;

                ProgressChanged?.Invoke(this, new DownloadProgress
                {
                    CurrentFileName = _currentFileName,
                    CurrentFileIndex = _currentFileIndex,
                    TotalFiles = _totalFiles,
                    BytesDownloaded = totalBytesRead,
                    TotalBytes = totalBytes,
                    SpeedBytesPerSecond = speed
                });

                lastProgressUpdate = DateTime.Now;
            }
        }

        // Final progress update
        var finalElapsed = stopwatch.Elapsed.TotalSeconds;
        var finalSpeed = finalElapsed > 0 ? totalBytesRead / finalElapsed : 0;

        ProgressChanged?.Invoke(this, new DownloadProgress
        {
            CurrentFileName = _currentFileName,
            CurrentFileIndex = _currentFileIndex,
            TotalFiles = _totalFiles,
            BytesDownloaded = totalBytesRead,
            TotalBytes = totalBytes,
            SpeedBytesPerSecond = finalSpeed
        });
    }

    public void CancelDownload()
    {
        _cancellationTokenSource?.Cancel();
    }

    public void Dispose()
    {
        _httpClient.Dispose();
        _cancellationTokenSource?.Dispose();
    }
}
