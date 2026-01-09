using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Net.Http;
using GameUpdater.Models;
using GameUpdater.Services.Interfaces;

namespace GameUpdater.Services;

/// <summary>
/// Service for checking and performing game updates
/// </summary>
public class UpdateService : IUpdateService
{
    private readonly IConfigurationService _configService;
    private readonly IDownloadService _downloadService;
    private readonly IHashService _hashService;
    private CancellationTokenSource? _cancellationTokenSource;
    private PatchFile? _cachedPatchFile;

    public event EventHandler<string>? StatusChanged;
    public event EventHandler<DownloadProgress>? ProgressChanged;

    public UpdateService(
        IConfigurationService configService,
        IDownloadService downloadService,
        IHashService hashService)
    {
        _configService = configService;
        _downloadService = downloadService;
        _hashService = hashService;

        _downloadService.ProgressChanged += (s, e) => ProgressChanged?.Invoke(this, e);
    }

    public async Task<bool> AreUpdatesAvailableAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            StatusChanged?.Invoke(this, "Checking for updates...");

            var patchFile = await GetPatchFileAsync(cancellationToken);
            if (patchFile == null || patchFile.Files.Count == 0)
            {
                StatusChanged?.Invoke(this, "No patch information available.");
                return false;
            }

            var filesNeedingUpdate = await GetFilesNeedingUpdateAsync(cancellationToken);
            return filesNeedingUpdate.Count > 0;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error checking for updates: {ex.Message}");
            StatusChanged?.Invoke(this, $"Error checking for updates: {ex.Message}");
            return false;
        }
    }

    public async Task<List<FileEntry>> GetFilesNeedingUpdateAsync(CancellationToken cancellationToken = default)
    {
        var filesToUpdate = new List<FileEntry>();
        var patchFile = await GetPatchFileAsync(cancellationToken);

        if (patchFile == null)
            return filesToUpdate;

        var gameDir = GetGameDirectory();

        foreach (var file in patchFile.Files)
        {
            var localPath = Path.Combine(gameDir, file.Path.Replace('/', Path.DirectorySeparatorChar));

            if (!File.Exists(localPath))
            {
                filesToUpdate.Add(file);
                continue;
            }

            var hashMatches = await _hashService.VerifyFileHashAsync(localPath, file.Hash);
            if (!hashMatches)
            {
                filesToUpdate.Add(file);
            }
        }

        return filesToUpdate;
    }

    public async Task PerformUpdateAsync(CancellationToken cancellationToken = default)
    {
        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var token = _cancellationTokenSource.Token;

        try
        {
            var filesToUpdate = await GetFilesNeedingUpdateAsync(token);

            if (filesToUpdate.Count == 0)
            {
                StatusChanged?.Invoke(this, "Game is up to date.");
                return;
            }

            StatusChanged?.Invoke(this, $"Downloading {filesToUpdate.Count} files...");

            var gameDir = GetGameDirectory();
            var baseUrl = _configService.Settings.LauncherSettings.UpdateBaseUrl.TrimEnd('/');

            for (int i = 0; i < filesToUpdate.Count; i++)
            {
                token.ThrowIfCancellationRequested();

                var file = filesToUpdate[i];
                var localPath = Path.Combine(gameDir, file.Path.Replace('/', Path.DirectorySeparatorChar));
                var downloadUrl = $"{baseUrl}/{file.Path}";

                StatusChanged?.Invoke(this, $"Downloading: {file.Path}");
                _downloadService.SetCurrentFileInfo(Path.GetFileName(file.Path), i + 1, filesToUpdate.Count);

                try
                {
                    await _downloadService.DownloadFileAsync(downloadUrl, localPath, file.Size, token);

                    // Verify downloaded file
                    var hashMatches = await _hashService.VerifyFileHashAsync(localPath, file.Hash);
                    if (!hashMatches)
                    {
                        Debug.WriteLine($"Hash mismatch after download: {file.Path}");
                        StatusChanged?.Invoke(this, $"Warning: Hash mismatch for {file.Path}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    Debug.WriteLine($"Failed to download {file.Path}: {ex.Message}");
                    StatusChanged?.Invoke(this, $"Failed to download: {file.Path}");
                }
            }

            StatusChanged?.Invoke(this, "Update completed successfully!");
        }
        catch (OperationCanceledException)
        {
            StatusChanged?.Invoke(this, "Update cancelled.");
            throw;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error during update: {ex.Message}");
            StatusChanged?.Invoke(this, $"Update failed: {ex.Message}");
            throw;
        }
    }

    public async Task RepairFilesAsync(CancellationToken cancellationToken = default)
    {
        _cachedPatchFile = null; // Force refresh
        StatusChanged?.Invoke(this, "Verifying game files...");

        var filesToRepair = await GetFilesNeedingUpdateAsync(cancellationToken);

        if (filesToRepair.Count == 0)
        {
            StatusChanged?.Invoke(this, "All files verified. No repairs needed.");
            return;
        }

        StatusChanged?.Invoke(this, $"Found {filesToRepair.Count} files to repair.");
        await PerformUpdateAsync(cancellationToken);
    }

    public void LaunchGame()
    {
        var gameDir = GetGameDirectory();
        var gameExe = Path.Combine(gameDir, _configService.Settings.LauncherSettings.GameExecutable);

        if (!File.Exists(gameExe))
        {
            StatusChanged?.Invoke(this, $"Game executable not found: {gameExe}");
            return;
        }

        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = gameExe,
                WorkingDirectory = gameDir,
                UseShellExecute = true
            };

            Process.Start(startInfo);
            StatusChanged?.Invoke(this, "Game launched successfully!");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error launching game: {ex.Message}");
            StatusChanged?.Invoke(this, $"Failed to launch game: {ex.Message}");
        }
    }

    public void CancelUpdate()
    {
        _cancellationTokenSource?.Cancel();
    }

    private async Task<PatchFile?> GetPatchFileAsync(CancellationToken cancellationToken)
    {
        if (_cachedPatchFile != null)
            return _cachedPatchFile;

        try
        {
            var patchUrl = _configService.Settings.LauncherSettings.PatchUrl;
            var json = await _downloadService.DownloadStringAsync(patchUrl, cancellationToken);

            _cachedPatchFile = JsonSerializer.Deserialize<PatchFile>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return _cachedPatchFile;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error fetching patch file: {ex.Message}");
            return null;
        }
    }

    private string GetGameDirectory()
    {
        return AppDomain.CurrentDomain.BaseDirectory;
    }
}
