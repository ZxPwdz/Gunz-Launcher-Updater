using System.Text.Json.Serialization;

namespace GameUpdater.Models;

/// <summary>
/// Represents the patch.json manifest file structure
/// </summary>
public class PatchFile
{
    [JsonPropertyName("Files")]
    public List<FileEntry> Files { get; set; } = new();

    [JsonPropertyName("TotalFiles")]
    public int TotalFiles { get; set; }

    [JsonPropertyName("TotalSize")]
    public long TotalSize { get; set; }
}
