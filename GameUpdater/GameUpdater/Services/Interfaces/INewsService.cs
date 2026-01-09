using GameUpdater.Models;

namespace GameUpdater.Services.Interfaces;

/// <summary>
/// Service for fetching news from the server
/// </summary>
public interface INewsService
{
    /// <summary>
    /// Get news items from the server
    /// </summary>
    Task<List<NewsItem>> GetNewsAsync(CancellationToken cancellationToken = default);
}
