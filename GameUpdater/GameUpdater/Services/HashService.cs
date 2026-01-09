using System.IO;
using System.Security.Cryptography;
using GameUpdater.Services.Interfaces;

namespace GameUpdater.Services;

/// <summary>
/// Service for computing and verifying file hashes
/// </summary>
public class HashService : IHashService
{
    public async Task<string> ComputeSHA256Async(string filePath)
    {
        using var sha256 = SHA256.Create();
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 8192, true);

        var hashBytes = await Task.Run(() => sha256.ComputeHash(stream));
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
    }

    public async Task<bool> VerifyFileHashAsync(string filePath, string expectedHash)
    {
        if (!File.Exists(filePath))
            return false;

        try
        {
            var actualHash = await ComputeSHA256Async(filePath);
            return string.Equals(actualHash, expectedHash, StringComparison.OrdinalIgnoreCase);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error verifying hash for {filePath}: {ex.Message}");
            return false;
        }
    }
}
