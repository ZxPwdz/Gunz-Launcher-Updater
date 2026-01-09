using System.Windows;
using System.Windows.Input;
using GameUpdater.Helpers;
using GameUpdater.Services.Interfaces;

namespace GameUpdater.Views.Dialogs;

/// <summary>
/// Settings dialog for configuring launcher options
/// </summary>
public partial class SettingsDialog : Window
{
    private readonly IConfigurationService _configService;

    public SettingsDialog(IConfigurationService configService)
    {
        InitializeComponent();
        _configService = configService;
        LoadSettings();
    }

    private void LoadSettings()
    {
        var settings = _configService.Settings;

        PatchUrlTextBox.Text = settings.LauncherSettings.PatchUrl;
        UpdateBaseUrlTextBox.Text = settings.LauncherSettings.UpdateBaseUrl;
        NewsUrlTextBox.Text = settings.LauncherSettings.NewsUrl;
        GameExecutableTextBox.Text = settings.LauncherSettings.GameExecutable;

        SkipUpdateCheckBox.IsChecked = settings.LauncherSettings.SkipUpdateOnStartup;
        MinimizeOnLaunchCheckBox.IsChecked = settings.LauncherSettings.MinimizeOnGameLaunch;
        CloseOnLaunchCheckBox.IsChecked = settings.LauncherSettings.CloseOnGameLaunch;
    }

    private void SaveSettings()
    {
        var settings = _configService.Settings;

        settings.LauncherSettings.PatchUrl = PatchUrlTextBox.Text;
        settings.LauncherSettings.UpdateBaseUrl = UpdateBaseUrlTextBox.Text;
        settings.LauncherSettings.NewsUrl = NewsUrlTextBox.Text;
        settings.LauncherSettings.GameExecutable = GameExecutableTextBox.Text;

        settings.LauncherSettings.SkipUpdateOnStartup = SkipUpdateCheckBox.IsChecked ?? false;
        settings.LauncherSettings.MinimizeOnGameLaunch = MinimizeOnLaunchCheckBox.IsChecked ?? false;
        settings.LauncherSettings.CloseOnGameLaunch = CloseOnLaunchCheckBox.IsChecked ?? false;

        _configService.Save();
    }

    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        WindowDragHelper.DragWindow(this);
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        SaveSettings();
        DialogResult = true;
        Close();
    }

    private void ResetButton_Click(object sender, RoutedEventArgs e)
    {
        _configService.ResetToDefaults();
        LoadSettings();
    }
}
