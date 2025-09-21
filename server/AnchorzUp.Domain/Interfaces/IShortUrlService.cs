using AnchorzUp.Domain.Entities;

namespace AnchorzUp.Domain.Interfaces;

public interface IShortUrlService
{
    Task<ShortUrl> CreateShortUrlAsync(string originalUrl, DateTime? expiresAt = null);
    Task<ShortUrl?> GetOriginalUrlAsync(string shortCode);
    Task DeleteShortUrlAsync(Guid id);
    Task<IEnumerable<ShortUrl>> GetAllShortUrlsAsync();
    Task<ShortUrl> TrackClickAsync(string shortCode, string? ipAddress = null, string? userAgent = null, string? referer = null);
    string GenerateShortCode();
}
