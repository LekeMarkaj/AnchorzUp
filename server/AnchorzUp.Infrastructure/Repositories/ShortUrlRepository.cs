using AnchorzUp.Application.Interfaces;
using AnchorzUp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using AnchorzUp.Application.ShortUrl.DTOs;

namespace AnchorzUp.Infrastructure.Repositories;

public class ShortUrlRepository : IShortUrlRepository
{
    private readonly AnchorzUpDbContext _context;

    public ShortUrlRepository(AnchorzUpDbContext context)
    {
        _context = context;
    }

    public async Task<ShortUrlDto?> GetByShortCodeAsync(string shortCode)
    {
        var entity = await _context.ShortUrls
            .Include(s => s.Clicks)
            .FirstOrDefaultAsync(s => s.ShortCode == shortCode && s.IsActive);
        
        return entity != null ? MapToDto(entity) : null;
    }


    public async Task<ShortUrlDto> CreateAsync(ShortUrlDto shortUrl)
    {
        var entity = MapToEntity(shortUrl);
        _context.ShortUrls.Add(entity);
        await _context.SaveChangesAsync();
        return MapToDto(entity);
    }

    public async Task<ShortUrlDto> UpdateAsync(ShortUrlDto shortUrl)
    {
        var entity = MapToEntity(shortUrl);
        _context.ShortUrls.Update(entity);
        await _context.SaveChangesAsync();
        return MapToDto(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        var shortUrl = await _context.ShortUrls.FindAsync(id);
        if (shortUrl != null)
        {
            shortUrl.IsActive = false;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<ShortUrlDto>> GetAllAsync()
    {
        var entities = await _context.ShortUrls
            .Where(s => s.IsActive)
            .Include(s => s.Clicks)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
        
        return entities.Select(MapToDto);
    }

    public async Task<bool> ShortCodeExistsAsync(string shortCode)
    {
        return await _context.ShortUrls
            .AnyAsync(s => s.ShortCode == shortCode && s.IsActive);
    }

    public async Task IncrementClickCountAsync(Guid shortUrlId)
    {
        var shortUrl = await _context.ShortUrls.FindAsync(shortUrlId);
        if (shortUrl != null)
        {
            shortUrl.ClickCount++;
            shortUrl.LastAccessedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task AddClickAsync(ClickDto click)
    {
        var entity = MapToEntity(click);
        _context.Clicks.Add(entity);
        await _context.SaveChangesAsync();
    }

    private static ShortUrlDto MapToDto(AnchorzUp.Domain.Entities.ShortUrl entity)
    {
        return new ShortUrlDto
        {
            Id = entity.Id,
            OriginalUrl = entity.OriginalUrl,
            ShortCode = entity.ShortCode,
            CreatedAt = entity.CreatedAt,
            ExpiresAt = entity.ExpiresAt,
            ClickCount = entity.ClickCount,
            LastAccessedAt = entity.LastAccessedAt,
            ShortUrl = string.Empty, // Will be set by the service layer
            QrCodeBase64 = string.Empty // Will be set by the service layer
        };
    }

    private static AnchorzUp.Domain.Entities.ShortUrl MapToEntity(ShortUrlDto dto)
    {
        return new AnchorzUp.Domain.Entities.ShortUrl
        {
            Id = dto.Id,
            OriginalUrl = dto.OriginalUrl,
            ShortCode = dto.ShortCode,
            CreatedAt = dto.CreatedAt,
            ExpiresAt = dto.ExpiresAt,
            ClickCount = dto.ClickCount,
            LastAccessedAt = dto.LastAccessedAt,
            IsActive = true
        };
    }

    private static AnchorzUp.Domain.Entities.Click MapToEntity(ClickDto dto)
    {
        return new AnchorzUp.Domain.Entities.Click
        {
            Id = dto.Id,
            ShortUrlId = dto.ShortUrlId,
            ClickedAt = dto.ClickedAt,
            IpAddress = dto.IpAddress,
            UserAgent = dto.UserAgent,
            Referer = dto.Referer
        };
    }
}
