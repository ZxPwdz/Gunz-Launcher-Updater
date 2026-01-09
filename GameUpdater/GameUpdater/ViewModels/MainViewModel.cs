using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using GameUpdater.Models;
using GameUpdater.Services.Interfaces;
using GameUpdater.ViewModels.Base;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace GameUpdater.ViewModels;

/// <summary>
/// Main ViewModel for the launcher window
/// </summary>
public class MainViewModel : ViewModelBase
{
    private readonly IUpdateService _updateService;
    private readonly IConfigurationService _configService;
    private readonly ISerialKeyService _serialKeyService;
    private readonly IPermissionService _permissionService;
    private readonly INewsService _newsService;

    #region Properties

    private string _statusText = "Ready";
    public string StatusText
    {
        get => _statusText;
        set => SetProperty(ref _statusText, value);
    }

    private string _downloadSpeedText = "";
    public string DownloadSpeedText
    {
        get => _downloadSpeedText;
        set => SetProperty(ref _downloadSpeedText, value);
    }

    private double _overallProgress;
    public double OverallProgress
    {
        get => _overallProgress;
        set => SetProperty(ref _overallProgress, value);
    }

    private double _fileProgress;
    public double FileProgress
    {
        get => _fileProgress;
        set => SetProperty(ref _fileProgress, value);
    }

    private bool _isUpdating;
    public bool IsUpdating
    {
        get => _isUpdating;
        set
        {
            if (SetProperty(ref _isUpdating, value))
            {
                OnPropertyChanged(nameof(CanLaunchGame));
            }
        }
    }

    private bool _isInitializing = true;
    public bool IsInitializing
    {
        get => _isInitializing;
        set
        {
            if (SetProperty(ref _isInitializing, value))
            {
                OnPropertyChanged(nameof(CanLaunchGame));
            }
        }
    }

    public bool CanLaunchGame => !IsUpdating && !IsInitializing;

    private string _currentFileName = string.Empty;
    public string CurrentFileName
    {
        get => _currentFileName;
        set => SetProperty(ref _currentFileName, value);
    }

    private string _progressText = "";
    public string ProgressText
    {
        get => _progressText;
        set => SetProperty(ref _progressText, value);
    }

    private ObservableCollection<NewsItem> _newsItems = new();
    public ObservableCollection<NewsItem> NewsItems
    {
        get => _newsItems;
        set => SetProperty(ref _newsItems, value);
    }

    public string WindowTitle => "Gunz Online Launcher";

    #endregion

    #region Commands

    public ICommand LaunchGameCommand { get; }
    public ICommand OpenSettingsCommand { get; }
    public ICommand RepairFilesCommand { get; }
    public ICommand OpenWebsiteCommand { get; }
    public ICommand OpenShopCommand { get; }
    public ICommand OpenSupportCommand { get; }
    public ICommand OpenDiscordCommand { get; }
    public ICommand OpenYoutubeCommand { get; }
    public ICommand OpenFacebookCommand { get; }
    public ICommand OpenInstagramCommand { get; }
    public ICommand OpenTwitterCommand { get; }
    public ICommand MinimizeCommand { get; }
    public ICommand CloseCommand { get; }

    #endregion

    public MainViewModel(
        IUpdateService updateService,
        IConfigurationService configService,
        ISerialKeyService serialKeyService,
        IPermissionService permissionService,
        INewsService newsService)
    {
        _updateService = updateService;
        _configService = configService;
        _serialKeyService = serialKeyService;
        _permissionService = permissionService;
        _newsService = newsService;

        // Initialize commands
        LaunchGameCommand = new AsyncRelayCommand(LaunchGameAsync, () => CanLaunchGame);
        OpenSettingsCommand = new RelayCommand(OpenSettings);
        RepairFilesCommand = new AsyncRelayCommand(RepairFilesAsync, () => !IsUpdating);

        // Social commands
        OpenWebsiteCommand = new RelayCommand(() => OpenUrl(_configService.Settings.SocialLinks.WebsiteUrl));
        OpenShopCommand = new RelayCommand(() => OpenUrl(_configService.Settings.SocialLinks.ShopUrl));
        OpenSupportCommand = new RelayCommand(() => OpenUrl(_configService.Settings.SocialLinks.SupportUrl));
        OpenDiscordCommand = new RelayCommand(() => OpenUrl(_configService.Settings.SocialLinks.DiscordUrl));
        OpenYoutubeCommand = new RelayCommand(() => OpenUrl(_configService.Settings.SocialLinks.YoutubeUrl));
        OpenFacebookCommand = new RelayCommand(() => OpenUrl(_configService.Settings.SocialLinks.FacebookUrl));
        OpenInstagramCommand = new RelayCommand(() => OpenUrl(_configService.Settings.SocialLinks.InstagramUrl));
        OpenTwitterCommand = new RelayCommand(() => OpenUrl(_configService.Settings.SocialLinks.TwitterUrl));

        // Window commands
        MinimizeCommand = new RelayCommand(MinimizeWindow);
        CloseCommand = new RelayCommand(CloseApplication);

        // Subscribe to events
        _updateService.StatusChanged += OnStatusChanged;
        _updateService.ProgressChanged += OnProgressChanged;
    }

    public async Task InitializeAsync()
    {
        IsInitializing = true;

        try
        {
            StatusText = "Initializing...";

            // Load configuration
            _configService.Load();

            // Extract SKGen.dll
            await _serialKeyService.ExtractSKGenAsync();

            // Load news
            await LoadNewsAsync();

            // Check for updates
            if (!_configService.Settings.LauncherSettings.SkipUpdateOnStartup)
            {
                await CheckForUpdatesAsync();
            }
            else
            {
                StatusText = "Ready to play!";
            }
        }
        catch (Exception ex)
        {
            StatusText = $"Initialization error: {ex.Message}";
            Debug.WriteLine($"Initialization error: {ex}");
        }
        finally
        {
            IsInitializing = false;
        }
    }

    private async Task LoadNewsAsync()
    {
        try
        {
            var news = await _newsService.GetNewsAsync();
            NewsItems = new ObservableCollection<NewsItem>(news);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading news: {ex.Message}");
        }
    }

    private async Task CheckForUpdatesAsync()
    {
        try
        {
            StatusText = "Checking for updates...";

            if (!_permissionService.HasWritePermission())
            {
                if (_permissionService.IsRunningAsAdmin())
                {
                    _permissionService.TryGrantDirectoryPermissions();
                }
                else
                {
                    StatusText = "Administrator rights required for updates.";
                    return;
                }
            }

            bool updatesAvailable = await _updateService.AreUpdatesAvailableAsync();

            if (updatesAvailable)
            {
                await PerformUpdateAsync();
            }
            else
            {
                StatusText = "Game is up to date!";
            }
        }
        catch (Exception ex)
        {
            StatusText = $"Update check failed: {ex.Message}";
        }
    }

    private async Task PerformUpdateAsync()
    {
        IsUpdating = true;

        try
        {
            await _updateService.PerformUpdateAsync();
            StatusText = "Update complete! Ready to play.";
        }
        catch (OperationCanceledException)
        {
            StatusText = "Update cancelled.";
        }
        catch (Exception ex)
        {
            StatusText = $"Update failed: {ex.Message}";
        }
        finally
        {
            IsUpdating = false;
            OverallProgress = 0;
            FileProgress = 0;
            DownloadSpeedText = "";
        }
    }

    private async Task LaunchGameAsync()
    {
        try
        {
            StatusText = "Preparing to launch...";

            // Quick update check
            bool updatesAvailable = await _updateService.AreUpdatesAvailableAsync();
            if (updatesAvailable)
            {
                await PerformUpdateAsync();
            }

            // Generate serial key
            StatusText = "Initializing game...";
            bool keyGenerated = _serialKeyService.CreateSerialKey();

            if (!keyGenerated)
            {
                Debug.WriteLine("Serial key generation returned false (may be expected)");
            }

            // Launch game
            _updateService.LaunchGame();
            StatusText = "Game launched!";

            // Handle post-launch behavior
            if (_configService.Settings.LauncherSettings.CloseOnGameLaunch)
            {
                CloseApplication();
            }
            else if (_configService.Settings.LauncherSettings.MinimizeOnGameLaunch)
            {
                MinimizeWindow();
            }
        }
        catch (Exception ex)
        {
            StatusText = $"Failed to launch: {ex.Message}";
        }
    }

    private async Task RepairFilesAsync()
    {
        IsUpdating = true;

        try
        {
            await _updateService.RepairFilesAsync();
        }
        catch (Exception ex)
        {
            StatusText = $"Repair failed: {ex.Message}";
        }
        finally
        {
            IsUpdating = false;
            OverallProgress = 0;
            FileProgress = 0;
        }
    }

    private void OpenSettings()
    {
        // TODO: Open settings dialog
        MessageBox.Show("Settings dialog coming soon!", "Settings", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void OpenUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
            return;

        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error opening URL {url}: {ex.Message}");
        }
    }

    private void MinimizeWindow()
    {
        if (Application.Current.MainWindow != null)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
    }

    private void CloseApplication()
    {
        _serialKeyService.Cleanup();
        Application.Current.Shutdown();
    }

    private void OnStatusChanged(object? sender, string status)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            StatusText = status;
        });
    }

    private void OnProgressChanged(object? sender, DownloadProgress progress)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            OverallProgress = progress.OverallProgressPercent;
            FileProgress = progress.FileProgressPercent;
            CurrentFileName = progress.CurrentFileName;
            DownloadSpeedText = $"Speed: {progress.FormattedSpeed}";
            ProgressText = $"File {progress.CurrentFileIndex} of {progress.TotalFiles}";
        });
    }
}
