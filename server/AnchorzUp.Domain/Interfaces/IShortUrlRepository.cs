using AnchorzUp.Domain.Entities;

namespace AnchorzUp.Domain.Interfaces;

public interface IShortUrlRepository
{
    Task<ShortUrl?> GetByShortCodeAsync(string shortCode);
    Task<ShortUrl> CreateAsync(ShortUrl shortUrl);
    Task<ShortUrl> UpdateAsync(ShortUrl shortUrl);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<ShortUrl>> GetAllAsync();
    Task<bool> ShortCodeExistsAsync(string shortCode);
    Task IncrementClickCountAsync(Guid shortUrlId);
    Task AddClickAsync(Click click);
}
