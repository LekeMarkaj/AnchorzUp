using AnchorzUp.Application.Interfaces;
using AnchorzUp.Domain.Entities;
using AnchorzUp.Application.ShortUrl.DTOs;

namespace AnchorzUp.Infrastructure.Services;

public class ShortUrlService : IShortUrlService
{
    private readonly IShortUrlRepository _repository;
    private const int ShortCodeLength = 8;

    public ShortUrlService(IShortUrlRepository repository)
    {
        _repository = repository;
    }

    public async Task<ShortUrlDto> CreateShortUrlAsync(string originalUrl, DateTime? expiresAt = null)
    {
        // Validate URL
        if (!Uri.TryCreate(originalUrl, UriKind.Absolute, out var uri) || 
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            throw new ArgumentException("Invalid URL provided");
        }

        // Ensure expiration date is in UTC and set default expiration (30 days from now)
        var expirationDate = expiresAt?.Kind == DateTimeKind.Utc 
            ? expiresAt.Value 
            : expiresAt?.ToUniversalTime() ?? DateTime.UtcNow.AddDays(30);

        // Validate that expiration date is in the future (with 5 second buffer to prevent race conditions)
        if (expirationDate <= DateTime.UtcNow.AddSeconds(5))
        {
            throw new ArgumentException("Expiration date must be at least 5 seconds in the future");
        }

        var shortCode = await GenerateUniqueShortCodeAsync();

        var shortUrl = new ShortUrlDto
        {
            Id = Guid.NewGuid(),
            OriginalUrl = originalUrl,
            ShortCode = shortCode,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = expirationDate,
            ClickCount = 0
        };

        return await _repository.CreateAsync(shortUrl);
    }

    public async Task<ShortUrlDto?> GetOriginalUrlAsync(string shortCode)
    {
        return await _repository.GetByShortCodeAsync(shortCode);
    }


    public async Task DeleteShortUrlAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<ShortUrlDto>> GetAllShortUrlsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<ShortUrlDto> TrackClickAsync(string shortCode, string? ipAddress = null, string? userAgent = null, string? referer = null)
    {
        var shortUrl = await _repository.GetByShortCodeAsync(shortCode);
        if (shortUrl == null)
        {
            throw new ArgumentException("Short URL not found");
        }

        // Check if URL is expired
        if (IsUrlExpired(shortUrl))
        {
            throw new InvalidOperationException("Short URL has expired");
        }

        // Increment click count
        await _repository.IncrementClickCountAsync(shortUrl.Id);

        // Add click record
        var click = new ClickDto
        {
            Id = Guid.NewGuid(),
            ShortUrlId = shortUrl.Id,
            ClickedAt = DateTime.UtcNow,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            Referer = referer
        };

        await _repository.AddClickAsync(click);

        return shortUrl;
    }


    public string GenerateShortCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, ShortCodeLength)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private async Task<string> GenerateUniqueShortCodeAsync()
    {
        string shortCode;
        do
        {
            shortCode = GenerateShortCode();
        } while (await _repository.ShortCodeExistsAsync(shortCode));

        return shortCode;
    }

    private bool IsUrlExpired(ShortUrlDto shortUrl)
    {
        return shortUrl.ExpiresAt.HasValue && shortUrl.ExpiresAt.Value < DateTime.UtcNow;
    }
}
