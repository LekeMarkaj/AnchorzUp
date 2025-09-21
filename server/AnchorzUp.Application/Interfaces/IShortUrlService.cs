using AnchorzUp.Application.ShortUrl.DTOs;

namespace AnchorzUp.Application.Interfaces;

public interface IShortUrlService
{
    Task<ShortUrlDto> CreateShortUrlAsync(string originalUrl, DateTime? expiresAt = null);
    Task<ShortUrlDto?> GetOriginalUrlAsync(string shortCode);
    Task DeleteShortUrlAsync(Guid id);
    Task<IEnumerable<ShortUrlDto>> GetAllShortUrlsAsync();
    Task<ShortUrlDto> TrackClickAsync(string shortCode, string? ipAddress = null, string? userAgent = null, string? referer = null);
    string GenerateShortCode();
}
