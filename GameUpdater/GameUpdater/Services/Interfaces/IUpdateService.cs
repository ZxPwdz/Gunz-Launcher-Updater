using GameUpdater.Models;

namespace GameUpdater.Services.Interfaces;

/// <summary>
/// Service for checking and performing game updates
/// </summary>
public interface IUpdateService
{
    /// <summary>
    /// Event raised when update status changes
    /// </summary>
    event EventHandler<string>? StatusChanged;

    /// <summary>
    /// Event raised when download progress changes
    /// </summary>
    event EventHandler<DownloadProgress>? ProgressChanged;

    /// <summary>
    /// Check if any updates are available
    /// </summary>
    Task<bool> AreUpdatesAvailableAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Perform the update process
    /// </summary>
    Task PerformUpdateAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get list of files that need updating
    /// </summary>
    Task<List<FileEntry>> GetFilesNeedingUpdateAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Verify all game files and repair if necessary
    /// </summary>
    Task RepairFilesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Launch the game executable
    /// </summary>
    void LaunchGame();

    /// <summary>
    /// Cancel ongoing update
    /// </summary>
    void CancelUpdate();
}
