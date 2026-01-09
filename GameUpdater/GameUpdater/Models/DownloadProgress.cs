namespace GameUpdater.Models;

/// <summary>
/// Represents the current download progress state
/// </summary>
public class DownloadProgress
{
    /// <summary>
    /// Name of the file currently being downloaded
    /// </summary>
    public string CurrentFileName { get; set; } = string.Empty;

    /// <summary>
    /// Index of the current file (1-based for display)
    /// </summary>
    public int CurrentFileIndex { get; set; }

    /// <summary>
    /// Total number of files to download
    /// </summary>
    public int TotalFiles { get; set; }

    /// <summary>
    /// Bytes downloaded for the current file
    /// </summary>
    public long BytesDownloaded { get; set; }

    /// <summary>
    /// Total bytes for the current file
    /// </summary>
    public long TotalBytes { get; set; }

    /// <summary>
    /// Current download speed in bytes per second
    /// </summary>
    public double SpeedBytesPerSecond { get; set; }

    /// <summary>
    /// Total bytes downloaded across all files
    /// </summary>
    public long TotalBytesDownloaded { get; set; }

    /// <summary>
    /// Total bytes to download across all files
    /// </summary>
    public long TotalBytesAll { get; set; }

    /// <summary>
    /// Overall progress percentage (0-100)
    /// </summary>
    public double OverallProgressPercent => TotalFiles > 0
        ? ((CurrentFileIndex - 1) * 100.0 / TotalFiles) + (FileProgressPercent / TotalFiles)
        : 0;

    /// <summary>
    /// Current file progress percentage (0-100)
    /// </summary>
    public double FileProgressPercent => TotalBytes > 0
        ? (BytesDownloaded * 100.0 / TotalBytes)
        : 0;

    /// <summary>
    /// Formatted download speed string
    /// </summary>
    public string FormattedSpeed
    {
        get
        {
            if (SpeedBytesPerSecond >= 1024 * 1024)
                return $"{SpeedBytesPerSecond / (1024 * 1024):F2} MB/s";
            if (SpeedBytesPerSecond >= 1024)
                return $"{SpeedBytesPerSecond / 1024:F2} KB/s";
            return $"{SpeedBytesPerSecond:F0} B/s";
        }
    }

    /// <summary>
    /// Formatted current file size downloaded
    /// </summary>
    public string FormattedBytesDownloaded => FormatBytes(BytesDownloaded);

    /// <summary>
    /// Formatted total file size
    /// </summary>
    public string FormattedTotalBytes => FormatBytes(TotalBytes);

    private static string FormatBytes(long bytes)
    {
        if (bytes >= 1024 * 1024 * 1024)
            return $"{bytes / (1024.0 * 1024 * 1024):F2} GB";
        if (bytes >= 1024 * 1024)
            return $"{bytes / (1024.0 * 1024):F2} MB";
        if (bytes >= 1024)
            return $"{bytes / 1024.0:F2} KB";
        return $"{bytes} B";
    }
}
