using System.Diagnostics;
using System.Windows;
using Microsoft.Win32;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace GameUpdater;

/// <summary>
/// Main application class
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Enable modern IE rendering mode for WebBrowser control
        SetBrowserEmulationMode();

        // Set up global exception handling
        DispatcherUnhandledException += (s, args) =>
        {
            MessageBox.Show(
                $"An unexpected error occurred:\n\n{args.Exception.Message}",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            args.Handled = true;
        };
    }

    /// <summary>
    /// Sets the IE emulation mode for the WebBrowser control to use Edge mode (IE11)
    /// This allows modern JavaScript and CSS to work properly
    /// </summary>
    private void SetBrowserEmulationMode()
    {
        try
        {
            var exeName = Process.GetCurrentProcess().ProcessName + ".exe";

            using var key = Registry.CurrentUser.CreateSubKey(
                @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION",
                RegistryKeyPermissionCheck.ReadWriteSubTree);

            if (key != null)
            {
                // 11001 = IE11 Edge mode (best compatibility with modern web standards)
                key.SetValue(exeName, 11001, RegistryValueKind.DWord);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Could not set browser emulation mode: {ex.Message}");
            // Non-critical, continue without setting
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
    }
}
