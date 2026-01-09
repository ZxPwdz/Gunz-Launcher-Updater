namespace GameUpdater.Configuration;

/// <summary>
/// =======================================================================
/// LAUNCHER CONFIGURATION - EDIT THIS FILE TO CUSTOMIZE YOUR LAUNCHER
/// =======================================================================
///
/// This is the central configuration file for the Gunz Online Launcher.
/// All URLs, settings, and social links are configured here.
///
/// After making changes, rebuild the project to apply them.
/// =======================================================================
/// </summary>
public static class LauncherConfig
{
    // =====================================================================
    // SERVER & UPDATE SETTINGS
    // =====================================================================

    /// <summary>
    /// URL to the patch.json file that contains file update information
    /// Example: "http://yourserver.com/update/patch.json"
    /// </summary>
    public static string PatchUrl => "http://localhost/GunzWeb/Update/patch.json";

    /// <summary>
    /// Base URL for downloading update files (folder containing the files)
    /// Example: "http://yourserver.com/update/"
    /// </summary>
    public static string UpdateBaseUrl => "http://localhost/GunzWeb/Update/";

    /// <summary>
    /// URL to the news API endpoint
    /// Example: "http://yourserver.com/api/news.php"
    /// </summary>
    public static string NewsUrl => "http://localhost/GunzWeb/api/news.php";

    /// <summary>
    /// URL for the embedded web content displayed in the launcher
    /// Example: "http://yourserver.com/" or leave empty to show fallback
    /// </summary>
    public static string WebContentUrl => "http://localhost/GunzWeb/";

    /// <summary>
    /// Name of the game executable to launch
    /// </summary>
    public static string GameExecutable => "Gunz.exe";


    // =====================================================================
    // LAUNCHER BEHAVIOR SETTINGS
    // =====================================================================

    /// <summary>
    /// Skip checking for updates when launcher starts
    /// Set to true for testing, false for production
    /// </summary>
    public static bool SkipUpdateOnStartup => false;

    /// <summary>
    /// Minimize the launcher window when the game is launched
    /// </summary>
    public static bool MinimizeOnGameLaunch => true;

    /// <summary>
    /// Close the launcher completely when the game is launched
    /// If true, MinimizeOnGameLaunch is ignored
    /// </summary>
    public static bool CloseOnGameLaunch => false;


    // =====================================================================
    // SOCIAL MEDIA & WEBSITE LINKS
    // =====================================================================

    /// <summary>
    /// Main website URL (WEBSITE button in footer)
    /// </summary>
    public static string WebsiteUrl => "https://gunzonline.net";

    /// <summary>
    /// Shop/Store URL (SHOP button in footer)
    /// </summary>
    public static string ShopUrl => "https://gunzonline.net/shop";

    /// <summary>
    /// Support/Help URL (SUPPORT button in footer)
    /// </summary>
    public static string SupportUrl => "https://gunzonline.net/support";

    /// <summary>
    /// Discord invite link (leave empty to hide button)
    /// </summary>
    public static string DiscordUrl => "https://discord.gg/gunzonline";

    /// <summary>
    /// YouTube channel URL (leave empty to hide button)
    /// </summary>
    public static string YoutubeUrl => "";

    /// <summary>
    /// Facebook page URL (leave empty to hide button)
    /// </summary>
    public static string FacebookUrl => "";

    /// <summary>
    /// Instagram profile URL (leave empty to hide button)
    /// </summary>
    public static string InstagramUrl => "";

    /// <summary>
    /// Twitter/X profile URL (leave empty to hide button)
    /// </summary>
    public static string TwitterUrl => "";


    // =====================================================================
    // UI SETTINGS
    // =====================================================================

    /// <summary>
    /// Theme name (currently only "DarkGaming" is supported)
    /// </summary>
    public static string Theme => "DarkGaming";

    /// <summary>
    /// Accent color in hex format (used for highlights and buttons)
    /// Examples: "#FF8C00" (orange), "#00A3FF" (blue), "#7B2FFF" (purple)
    /// </summary>
    public static string AccentColor => "#FF8C00";

    /// <summary>
    /// Show the news panel on the right side of the launcher
    /// </summary>
    public static bool ShowNewsPanel => true;

    /// <summary>
    /// Enable UI animations (hover effects, transitions)
    /// </summary>
    public static bool AnimationsEnabled => true;
}
