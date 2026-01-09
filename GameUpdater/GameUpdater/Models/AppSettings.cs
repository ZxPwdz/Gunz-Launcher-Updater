namespace GameUpdater.Models;

/// <summary>
/// Application settings loaded from appsettings.json
/// </summary>
public class AppSettings
{
    public LauncherSettings LauncherSettings { get; set; } = new();
    public SocialLinks SocialLinks { get; set; } = new();
    public UISettings UISettings { get; set; } = new();
}

/// <summary>
/// Core launcher settings for update and game functionality
/// </summary>
public class LauncherSettings
{
    public string PatchUrl { get; set; } = "http://localhost/gunz/patch.json";
    public string UpdateBaseUrl { get; set; } = "http://localhost/gunz/update/";
    public string NewsUrl { get; set; } = "http://localhost/GunzWeb/api/news.php";
    public string WebContentUrl { get; set; } = "http://localhost/GunzWeb/";
    public string GameExecutable { get; set; } = "Gunz.exe";
    public bool SkipUpdateOnStartup { get; set; } = false;
    public bool MinimizeOnGameLaunch { get; set; } = true;
    public bool CloseOnGameLaunch { get; set; } = false;
}

/// <summary>
/// Social media and website URLs
/// </summary>
public class SocialLinks
{
    public string WebsiteUrl { get; set; } = "https://vgunz.net";
    public string ShopUrl { get; set; } = "https://vgunz.net/shop";
    public string SupportUrl { get; set; } = "https://vgunz.net/support";
    public string DiscordUrl { get; set; } = "https://discord.gg/vgunz";
    public string YoutubeUrl { get; set; } = "";
    public string FacebookUrl { get; set; } = "";
    public string InstagramUrl { get; set; } = "";
    public string TwitterUrl { get; set; } = "";
}

/// <summary>
/// UI customization settings
/// </summary>
public class UISettings
{
    public string Theme { get; set; } = "DarkGaming";
    public string AccentColor { get; set; } = "#00A3FF";
    public bool ShowNewsPanel { get; set; } = true;
    public bool AnimationsEnabled { get; set; } = true;
}
