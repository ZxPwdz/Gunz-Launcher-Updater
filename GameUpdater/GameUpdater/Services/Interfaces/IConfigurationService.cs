using GameUpdater.Models;

namespace GameUpdater.Services.Interfaces;

/// <summary>
/// Service for managing application configuration
/// </summary>
public interface IConfigurationService
{
    /// <summary>
    /// Current application settings
    /// </summary>
    AppSettings Settings { get; }

    /// <summary>
    /// Load settings from configuration file
    /// </summary>
    void Load();

    /// <summary>
    /// Save current settings to configuration file
    /// </summary>
    void Save();

    /// <summary>
    /// Reset settings to default values
    /// </summary>
    void ResetToDefaults();
}
