using System.Diagnostics;
using System.Text.Json;
using GameUpdater.Models;
using GameUpdater.Services.Interfaces;

namespace GameUpdater.Services;

/// <summary>
/// Service for fetching news from the server
/// </summary>
public class NewsService : INewsService
{
    private readonly IConfigurationService _configService;
    private readonly IDownloadService _downloadService;

    public NewsService(IConfigurationService configService, IDownloadService downloadService)
    {
        _configService = configService;
        _downloadService = downloadService;
    }

    public async Task<List<NewsItem>> GetNewsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var newsUrl = _configService.Settings.LauncherSettings.NewsUrl;

            if (string.IsNullOrEmpty(newsUrl))
                return GetDefaultNews();

            var json = await _downloadService.DownloadStringAsync(newsUrl, cancellationToken);

            // Try parsing as array first (new API format)
            try
            {
                var newsArray = JsonSerializer.Deserialize<List<NewsApiItem>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (newsArray != null && newsArray.Count > 0)
                {
                    return newsArray.Select(item => new NewsItem
                    {
                        Title = item.Title ?? "",
                        Summary = item.Summary ?? "",
                        Category = item.Category ?? "NEWS",
                        Date = DateTime.TryParse(item.Date, out var date) ? date : DateTime.Now
                    }).ToList();
                }
            }
            catch
            {
                // Try parsing as NewsFeed object (old format)
                var feed = JsonSerializer.Deserialize<NewsFeed>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (feed?.Items != null)
                    return feed.Items;
            }

            return GetDefaultNews();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error fetching news: {ex.Message}");
            return GetDefaultNews();
        }
    }

    private List<NewsItem> GetDefaultNews()
    {
        return new List<NewsItem>
        {
            new NewsItem
            {
                Title = "Welcome to Gunz Online!",
                Summary = "Thank you for playing Gunz Online. Check back here for the latest updates and news.",
                Category = "NEWS",
                Date = DateTime.Now
            }
        };
    }
}

/// <summary>
/// News item from the API
/// </summary>
internal class NewsApiItem
{
    public int Id { get; set; }
    public string? Category { get; set; }
    public string? Title { get; set; }
    public string? Summary { get; set; }
    public string? Date { get; set; }
    public string? Image { get; set; }
}
