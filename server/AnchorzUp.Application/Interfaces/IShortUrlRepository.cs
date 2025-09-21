using AnchorzUp.Application.ShortUrl.DTOs;

namespace AnchorzUp.Application.Interfaces;

public interface IShortUrlRepository
{
    Task<ShortUrlDto?> GetByShortCodeAsync(string shortCode);
    Task<ShortUrlDto> CreateAsync(ShortUrlDto shortUrl);
    Task<ShortUrlDto> UpdateAsync(ShortUrlDto shortUrl);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<ShortUrlDto>> GetAllAsync();
    Task<bool> ShortCodeExistsAsync(string shortCode);
    Task IncrementClickCountAsync(Guid shortUrlId);
    Task AddClickAsync(ClickDto click);
}
