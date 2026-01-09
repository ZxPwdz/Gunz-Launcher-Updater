using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using GameUpdater.Helpers;
using GameUpdater.Services;
using GameUpdater.Services.Interfaces;
using GameUpdater.ViewModels;
using Microsoft.Web.WebView2.Core;

namespace GameUpdater.Views;

/// <summary>
/// Main window of the Game Updater application
/// </summary>
public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel;
    private readonly IConfigurationService _configService;

    public MainWindow()
    {
        InitializeComponent();

        // Create services
        _configService = new ConfigurationService();
        var hashService = new HashService();
        var downloadService = new DownloadService();
        var updateService = new UpdateService(_configService, downloadService, hashService);
        var serialKeyService = new SerialKeyService();
        var permissionService = new PermissionService();
        var newsService = new NewsService(_configService, downloadService);

        // Create and set ViewModel
        _viewModel = new MainViewModel(
            updateService,
            _configService,
            serialKeyService,
            permissionService,
            newsService);

        DataContext = _viewModel;

        // Initialize async
        Loaded += async (s, e) =>
        {
            // Initialize WebView2 and load web content
            await InitializeWebViewAsync();

            await _viewModel.InitializeAsync();

            // Return focus to the main window so buttons are immediately clickable
            // WebView2 captures focus after loading, which prevents button clicks
            Dispatcher.BeginInvoke(new Action(() =>
            {
                this.Focus();
                Keyboard.ClearFocus();
            }), System.Windows.Threading.DispatcherPriority.Background);
        };

        // Cleanup on close
        Closed += (s, e) =>
        {
            ContentBrowser?.Dispose();
            serialKeyService.Cleanup();
        };
    }

    private async Task InitializeWebViewAsync()
    {
        try
        {
            // Load the web content URL from config
            _configService.Load();
            var webContentUrl = _configService.Settings.LauncherSettings.WebContentUrl;

            Debug.WriteLine($"Loading web content from: {webContentUrl}");

            if (!string.IsNullOrEmpty(webContentUrl))
            {
                // Store WebView2 data in a hidden folder in AppData instead of next to the exe
                var userDataFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "GunzOnline", "WebView2");

                var environment = await CoreWebView2Environment.CreateAsync(null, userDataFolder);
                await ContentBrowser.EnsureCoreWebView2Async(environment);

                // Configure WebView2 settings
                ContentBrowser.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
                ContentBrowser.CoreWebView2.Settings.AreDevToolsEnabled = false;
                ContentBrowser.CoreWebView2.Settings.IsStatusBarEnabled = false;

                // Handle navigation completed to return focus to main window
                ContentBrowser.NavigationCompleted += OnWebViewNavigationCompleted;

                // Show WebView2, hide fallback
                ContentBrowser.Visibility = Visibility.Visible;
                FallbackContent.Visibility = Visibility.Collapsed;

                // Navigate to the URL
                ContentBrowser.CoreWebView2.Navigate(webContentUrl);
            }
            else
            {
                Debug.WriteLine("No WebContentUrl configured");
                ShowFallbackContent();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading web content: {ex.Message}");
            ShowFallbackContent();
        }
    }

    private void ShowFallbackContent()
    {
        ContentBrowser.Visibility = Visibility.Collapsed;
        FallbackContent.Visibility = Visibility.Visible;
    }

    private void OnWebViewNavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
    {
        // Return focus to the main window after WebView2 finishes loading
        // This ensures the Play button and other controls are immediately clickable
        Dispatcher.BeginInvoke(new Action(() =>
        {
            this.Focus();
            Keyboard.ClearFocus();
        }), System.Windows.Threading.DispatcherPriority.Background);
    }

    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount == 2)
        {
            // Double-click to toggle maximize (disabled for fixed size)
            return;
        }

        WindowDragHelper.DragWindow(this);
    }
}
