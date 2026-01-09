using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using GameUpdater.Services.Interfaces;

namespace GameUpdater.Services;

/// <summary>
/// Service for handling SKGen.dll serial key generation
/// </summary>
public class SerialKeyService : ISerialKeyService
{
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr LoadLibrary(string dllToLoad);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool FreeLibrary(IntPtr hModule);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool MakeSerialKeyDelegate();

    private string? _skgenDllPath;
    private IntPtr _loadedModule = IntPtr.Zero;

    public async Task ExtractSKGenAsync()
    {
        await Task.Run(() =>
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "GameUpdater.Assets.Embedded.SKGen.dll";

                using var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream == null)
                {
                    System.Diagnostics.Debug.WriteLine("SKGen.dll resource not found in assembly.");
                    return;
                }

                // Try app directory first
                string appDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SKGen.dll");

                try
                {
                    using var fileStream = File.Create(appDirPath);
                    stream.CopyTo(fileStream);
                    _skgenDllPath = appDirPath;
                    System.Diagnostics.Debug.WriteLine($"SKGen.dll extracted to: {appDirPath}");
                    return;
                }
                catch (UnauthorizedAccessException)
                {
                    System.Diagnostics.Debug.WriteLine("Cannot write to app directory, trying temp...");
                }
                catch (IOException)
                {
                    System.Diagnostics.Debug.WriteLine("IO error writing to app directory, trying temp...");
                }

                // Fallback: TEMP directory
                stream.Position = 0;
                string tempPath = Path.Combine(Path.GetTempPath(), $"SKGen_{Guid.NewGuid():N}.dll");

                using (var tempStream = File.Create(tempPath))
                {
                    stream.CopyTo(tempStream);
                }

                _skgenDllPath = tempPath;
                System.Diagnostics.Debug.WriteLine($"SKGen.dll extracted to temp: {tempPath}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error extracting SKGen.dll: {ex.Message}");
            }
        });
    }

    public bool CreateSerialKey()
    {
        if (string.IsNullOrEmpty(_skgenDllPath) || !File.Exists(_skgenDllPath))
        {
            System.Diagnostics.Debug.WriteLine("SKGen.dll not available.");
            return false;
        }

        try
        {
            _loadedModule = LoadLibrary(_skgenDllPath);
            if (_loadedModule == IntPtr.Zero)
            {
                var error = Marshal.GetLastWin32Error();
                System.Diagnostics.Debug.WriteLine($"Failed to load SKGen.dll. Error: {error}");
                return false;
            }

            var procAddress = GetProcAddress(_loadedModule, "MakeSerialKey");
            if (procAddress == IntPtr.Zero)
            {
                var error = Marshal.GetLastWin32Error();
                System.Diagnostics.Debug.WriteLine($"Failed to find MakeSerialKey. Error: {error}");
                FreeLibrary(_loadedModule);
                _loadedModule = IntPtr.Zero;
                return false;
            }

            var makeSerialKey = Marshal.GetDelegateForFunctionPointer<MakeSerialKeyDelegate>(procAddress);
            var result = makeSerialKey();

            FreeLibrary(_loadedModule);
            _loadedModule = IntPtr.Zero;

            System.Diagnostics.Debug.WriteLine($"MakeSerialKey result: {result}");
            return result;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error creating serial key: {ex.Message}");

            if (_loadedModule != IntPtr.Zero)
            {
                FreeLibrary(_loadedModule);
                _loadedModule = IntPtr.Zero;
            }

            return false;
        }
    }

    public void Cleanup()
    {
        try
        {
            if (_loadedModule != IntPtr.Zero)
            {
                FreeLibrary(_loadedModule);
                _loadedModule = IntPtr.Zero;
            }

            if (!string.IsNullOrEmpty(_skgenDllPath) && File.Exists(_skgenDllPath))
            {
                File.Delete(_skgenDllPath);
                System.Diagnostics.Debug.WriteLine($"SKGen.dll deleted: {_skgenDllPath}");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error cleaning up SKGen.dll: {ex.Message}");
        }
    }
}
