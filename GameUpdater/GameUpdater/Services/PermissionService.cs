using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using GameUpdater.Services.Interfaces;

namespace GameUpdater.Services;

/// <summary>
/// Service for handling file system permissions and admin elevation
/// </summary>
public class PermissionService : IPermissionService
{
    public bool HasWritePermission()
    {
        var gameDir = GetGameDirectory();
        return CanWriteToDirectory(gameDir);
    }

    public bool IsRunningAsAdmin()
    {
        try
        {
            using var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        catch
        {
            return false;
        }
    }

    public bool RequestAdminElevation()
    {
        try
        {
            var exePath = Process.GetCurrentProcess().MainModule?.FileName;
            if (string.IsNullOrEmpty(exePath))
                return false;

            var startInfo = new ProcessStartInfo
            {
                FileName = exePath,
                UseShellExecute = true,
                Verb = "runas"
            };

            Process.Start(startInfo);

            // Exit current process
            Environment.Exit(0);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to request admin elevation: {ex.Message}");
            return false;
        }
    }

    public bool TryGrantDirectoryPermissions()
    {
        if (!IsRunningAsAdmin())
            return false;

        var gameDir = GetGameDirectory();

        // Try .NET DirectorySecurity approach
        if (TryGrantPermissionsViaAcl(gameDir))
            return true;

        // Try icacls as fallback
        return TryGrantPermissionsViaIcacls(gameDir);
    }

    public string GetGameDirectory()
    {
        return AppDomain.CurrentDomain.BaseDirectory;
    }

    private bool CanWriteToDirectory(string path)
    {
        try
        {
            var testFile = Path.Combine(path, $"write_test_{Guid.NewGuid():N}.tmp");

            using (var fs = File.Create(testFile, 1, FileOptions.DeleteOnClose))
            {
                fs.WriteByte(0);
            }

            return true;
        }
        catch (UnauthorizedAccessException)
        {
            return false;
        }
        catch (IOException)
        {
            return false;
        }
        catch
        {
            return false;
        }
    }

    private bool TryGrantPermissionsViaAcl(string path)
    {
        try
        {
            var dirInfo = new DirectoryInfo(path);
            var security = dirInfo.GetAccessControl();

            // Get current user
            var currentUser = WindowsIdentity.GetCurrent().Name;

            // Add full control for current user
            security.AddAccessRule(new FileSystemAccessRule(
                currentUser,
                FileSystemRights.FullControl,
                InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                PropagationFlags.None,
                AccessControlType.Allow));

            // Add full control for Users group
            security.AddAccessRule(new FileSystemAccessRule(
                new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null),
                FileSystemRights.FullControl,
                InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                PropagationFlags.None,
                AccessControlType.Allow));

            dirInfo.SetAccessControl(security);

            Debug.WriteLine($"Granted permissions via ACL for: {path}");
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to grant permissions via ACL: {ex.Message}");
            return false;
        }
    }

    private bool TryGrantPermissionsViaIcacls(string path)
    {
        try
        {
            // S-1-5-32-545 is BUILTIN\Users
            var args = $"\"{path}\" /grant *S-1-5-32-545:(OI)(CI)F /T /C /Q";

            var startInfo = new ProcessStartInfo
            {
                FileName = "icacls.exe",
                Arguments = args,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using var process = Process.Start(startInfo);
            process?.WaitForExit(30000);

            var exitCode = process?.ExitCode ?? -1;
            Debug.WriteLine($"icacls exit code: {exitCode}");

            return exitCode == 0;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to grant permissions via icacls: {ex.Message}");
            return false;
        }
    }
}
