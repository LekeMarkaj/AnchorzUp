using MediatR;
using AnchorzUp.Application.Interfaces;
using Microsoft.Extensions.Options;
using AnchorzUp.Application.Configuration;
using AnchorzUp.Application.ShortUrl.DTOs;

namespace AnchorzUp.Application.ShortUrl.Commands.CreateShortUrl;

public class CreateShortUrlCommandHandler : IRequestHandler<CreateShortUrlCommand, CreateShortUrlResponseDto>
{
    private readonly IShortUrlService _shortUrlService;
    private readonly IQrCodeService _qrCodeService;
    private readonly AppSettings _appSettings;

    public CreateShortUrlCommandHandler(IShortUrlService shortUrlService, IQrCodeService qrCodeService, IOptions<AppSettings> appSettings)
    {
        _shortUrlService = shortUrlService;
        _qrCodeService = qrCodeService;
        _appSettings = appSettings.Value;
    }

    public async Task<CreateShortUrlResponseDto> Handle(CreateShortUrlCommand request, CancellationToken cancellationToken)
    {
        var shortUrl = await _shortUrlService.CreateShortUrlAsync(request.OriginalUrl, request.ExpiresAt);
        var qrCodeBase64 = await _qrCodeService.GetQrCodeAsBase64Async($"{_appSettings.BaseUrl}{shortUrl.ShortCode}");

        return new CreateShortUrlResponseDto
        {
            Id = shortUrl.Id,
            OriginalUrl = shortUrl.OriginalUrl,
            ShortUrl = $"{_appSettings.BaseUrl}{shortUrl.ShortCode}",
            ShortCode = shortUrl.ShortCode,
            CreatedAt = shortUrl.CreatedAt,
            ExpiresAt = shortUrl.ExpiresAt,
            ClickCount = shortUrl.ClickCount,
            QrCodeBase64 = qrCodeBase64
        };
    }
}
