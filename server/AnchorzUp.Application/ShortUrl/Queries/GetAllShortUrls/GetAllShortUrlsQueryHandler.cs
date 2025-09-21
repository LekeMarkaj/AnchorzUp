using MediatR;
using AnchorzUp.Application.Interfaces;
using Microsoft.Extensions.Options;
using AnchorzUp.Application.Configuration;
using AnchorzUp.Application.ShortUrl.DTOs;

namespace AnchorzUp.Application.ShortUrl.Queries.GetAllShortUrls;

public class GetAllShortUrlsQueryHandler : IRequestHandler<GetAllShortUrlsQuery, IEnumerable<ShortUrlDto>>
{
    private readonly IShortUrlService _shortUrlService;
    private readonly IQrCodeService _qrCodeService;
    private readonly AppSettings _appSettings;

    public GetAllShortUrlsQueryHandler(IShortUrlService shortUrlService, IQrCodeService qrCodeService, IOptions<AppSettings> appSettings)
    {
        _shortUrlService = shortUrlService;
        _qrCodeService = qrCodeService;
        _appSettings = appSettings.Value;
    }

    public async Task<IEnumerable<ShortUrlDto>> Handle(GetAllShortUrlsQuery request, CancellationToken cancellationToken)
    {
        var shortUrls = await _shortUrlService.GetAllShortUrlsAsync();
        
        var result = new List<ShortUrlDto>();
        foreach (var shortUrl in shortUrls)
        {
            var qrCodeBase64 = await _qrCodeService.GetQrCodeAsBase64Async($"{_appSettings.BaseUrl}{shortUrl.ShortCode}");
            result.Add(new ShortUrlDto
            {
                Id = shortUrl.Id,
                OriginalUrl = shortUrl.OriginalUrl,
                ShortUrl = $"{_appSettings.BaseUrl}{shortUrl.ShortCode}",
                ShortCode = shortUrl.ShortCode,
                CreatedAt = shortUrl.CreatedAt,
                ExpiresAt = shortUrl.ExpiresAt,
                ClickCount = shortUrl.ClickCount,
                LastAccessedAt = shortUrl.LastAccessedAt,
                QrCodeBase64 = qrCodeBase64
            });
        }
        
        return result;
    }
}
