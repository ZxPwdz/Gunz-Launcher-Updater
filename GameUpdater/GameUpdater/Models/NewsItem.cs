using System.Text.Json.Serialization;

namespace GameUpdater.Models;

/// <summary>
/// Represents a news item for display in the launcher
/// </summary>
public class NewsItem
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("summary")]
    public string Summary { get; set; } = string.Empty;

    [JsonPropertyName("imageUrl")]
    public string ImageUrl { get; set; } = string.Empty;

    [JsonPropertyName("linkUrl")]
    public string LinkUrl { get; set; } = string.Empty;

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("category")]
    public string Category { get; set; } = "News";

    /// <summary>
    /// Formatted date string for display
    /// </summary>
    public string FormattedDate => Date.ToString("MMM dd, yyyy");
}

/// <summary>
/// News feed response from server
/// </summary>
public class NewsFeed
{
    [JsonPropertyName("items")]
    public List<NewsItem> Items { get; set; } = new();
}
