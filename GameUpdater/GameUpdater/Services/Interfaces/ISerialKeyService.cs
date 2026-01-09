namespace GameUpdater.Services.Interfaces;

/// <summary>
/// Service for handling SKGen.dll serial key generation
/// </summary>
public interface ISerialKeyService
{
    /// <summary>
    /// Extract SKGen.dll from embedded resources
    /// </summary>
    Task ExtractSKGenAsync();

    /// <summary>
    /// Create serial key using SKGen.dll
    /// </summary>
    /// <returns>True if successful</returns>
    bool CreateSerialKey();

    /// <summary>
    /// Clean up extracted SKGen.dll
    /// </summary>
    void Cleanup();
}
