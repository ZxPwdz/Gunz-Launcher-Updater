using System.Text.Json.Serialization;

namespace GameUpdater.Models;

/// <summary>
/// Represents a single file entry in the patch manifest
/// </summary>
public class FileEntry
{
    [JsonPropertyName("Path")]
    public string Path { get; set; } = string.Empty;

    [JsonPropertyName("Size")]
    public long Size { get; set; }

    [JsonPropertyName("Hash")]
    public string Hash { get; set; } = string.Empty;
}
