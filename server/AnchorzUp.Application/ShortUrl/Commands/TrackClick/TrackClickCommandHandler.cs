using MediatR;
using AnchorzUp.Application.Interfaces;
using Microsoft.Extensions.Options;
using AnchorzUp.Application.Configuration;
using AnchorzUp.Application.ShortUrl.DTOs;

namespace AnchorzUp.Application.ShortUrl.Commands.TrackClick;

public class TrackClickCommandHandler : IRequestHandler<TrackClickCommand, ShortUrlDto>
{
    private readonly IShortUrlService _shortUrlService;
    private readonly IQrCodeService _qrCodeService;
    private readonly AppSettings _appSettings;

    public TrackClickCommandHandler(IShortUrlService shortUrlService, IQrCodeService qrCodeService, IOptions<AppSettings> appSettings)
    {
        _shortUrlService = shortUrlService;
        _qrCodeService = qrCodeService;
        _appSettings = appSettings.Value;
    }

    public async Task<ShortUrlDto> Handle(TrackClickCommand request, CancellationToken cancellationToken)
    {
        var shortUrl = await _shortUrlService.TrackClickAsync(
            request.ShortCode, 
            request.IpAddress, 
            request.UserAgent, 
            request.Referer);

        var qrCodeBase64 = await _qrCodeService.GetQrCodeAsBase64Async($"{_appSettings.BaseUrl}{shortUrl.ShortCode}");

        return new ShortUrlDto
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
        };
    }
}
