using GameUpdater.Models;

namespace GameUpdater.Services.Interfaces;

/// <summary>
/// Service for downloading files with progress reporting
/// </summary>
public interface IDownloadService
{
    /// <summary>
    /// Event raised when download progress changes
    /// </summary>
    event EventHandler<DownloadProgress>? ProgressChanged;

    /// <summary>
    /// Download a string from a URL
    /// </summary>
    Task<string> DownloadStringAsync(string url, CancellationToken cancellationToken = default);

    /// <summary>
    /// Download a file with progress reporting
    /// </summary>
    Task DownloadFileAsync(string url, string localPath, long expectedSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Set current file info for progress reporting
    /// </summary>
    void SetCurrentFileInfo(string fileName, int currentIndex, int totalFiles);

    /// <summary>
    /// Cancel any ongoing downloads
    /// </summary>
    void CancelDownload();
}
