using GameUpdater.Configuration;
using GameUpdater.Models;
using GameUpdater.Services.Interfaces;

namespace GameUpdater.Services;

/// <summary>
/// Service for managing application configuration
/// Configuration is now hardcoded in LauncherConfig.cs for security
/// </summary>
public class ConfigurationService : IConfigurationService
{
    private AppSettings _settings = new();

    public AppSettings Settings => _settings;

    public void Load()
    {
        // Load settings from hardcoded LauncherConfig class
        _settings = new AppSettings
        {
            LauncherSettings = new LauncherSettings
            {
                PatchUrl = LauncherConfig.PatchUrl,
                UpdateBaseUrl = LauncherConfig.UpdateBaseUrl,
                NewsUrl = LauncherConfig.NewsUrl,
                WebContentUrl = LauncherConfig.WebContentUrl,
                GameExecutable = LauncherConfig.GameExecutable,
                SkipUpdateOnStartup = LauncherConfig.SkipUpdateOnStartup,
                MinimizeOnGameLaunch = LauncherConfig.MinimizeOnGameLaunch,
                CloseOnGameLaunch = LauncherConfig.CloseOnGameLaunch
            },
            SocialLinks = new SocialLinks
            {
                WebsiteUrl = LauncherConfig.WebsiteUrl,
                ShopUrl = LauncherConfig.ShopUrl,
                SupportUrl = LauncherConfig.SupportUrl,
                DiscordUrl = LauncherConfig.DiscordUrl,
                YoutubeUrl = LauncherConfig.YoutubeUrl,
                FacebookUrl = LauncherConfig.FacebookUrl,
                InstagramUrl = LauncherConfig.InstagramUrl,
                TwitterUrl = LauncherConfig.TwitterUrl
            },
            UISettings = new UISettings
            {
                Theme = LauncherConfig.Theme,
                AccentColor = LauncherConfig.AccentColor,
                ShowNewsPanel = LauncherConfig.ShowNewsPanel,
                AnimationsEnabled = LauncherConfig.AnimationsEnabled
            }
        };
    }

    public void Save()
    {
        // Configuration is hardcoded - nothing to save
        // Edit LauncherConfig.cs to change settings
    }

    public void ResetToDefaults()
    {
        Load(); // Just reload from hardcoded values
    }
}
