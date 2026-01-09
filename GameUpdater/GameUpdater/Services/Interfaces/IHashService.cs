namespace GameUpdater.Services.Interfaces;

/// <summary>
/// Service for computing and verifying file hashes
/// </summary>
public interface IHashService
{
    /// <summary>
    /// Compute SHA256 hash of a file
    /// </summary>
    /// <param name="filePath">Path to the file</param>
    /// <returns>Lowercase hex string of the hash</returns>
    Task<string> ComputeSHA256Async(string filePath);

    /// <summary>
    /// Verify a file's hash matches the expected value
    /// </summary>
    /// <param name="filePath">Path to the file</param>
    /// <param name="expectedHash">Expected hash value</param>
    /// <returns>True if hash matches</returns>
    Task<bool> VerifyFileHashAsync(string filePath, string expectedHash);
}
