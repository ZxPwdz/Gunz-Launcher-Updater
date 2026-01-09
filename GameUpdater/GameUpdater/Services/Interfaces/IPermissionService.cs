namespace GameUpdater.Services.Interfaces;

/// <summary>
/// Service for handling file system permissions and admin elevation
/// </summary>
public interface IPermissionService
{
    /// <summary>
    /// Check if the application has write permission to the game directory
    /// </summary>
    bool HasWritePermission();

    /// <summary>
    /// Check if the application is running with administrator privileges
    /// </summary>
    bool IsRunningAsAdmin();

    /// <summary>
    /// Request admin elevation by restarting the application
    /// </summary>
    /// <returns>True if elevation was initiated</returns>
    bool RequestAdminElevation();

    /// <summary>
    /// Attempt to grant write permissions to the game directory
    /// </summary>
    /// <returns>True if permissions were granted</returns>
    bool TryGrantDirectoryPermissions();

    /// <summary>
    /// Get the game installation directory
    /// </summary>
    string GetGameDirectory();
}
