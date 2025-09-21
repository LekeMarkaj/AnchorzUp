using MediatR;
using AnchorzUp.Application.Interfaces;
using Microsoft.Extensions.Options;
using AnchorzUp.Application.Configuration;

namespace AnchorzUp.Application.ShortUrl.Queries.GetQrCode;

public class GetQrCodeQueryHandler : IRequestHandler<GetQrCodeQuery, byte[]>
{
    private readonly IQrCodeService _qrCodeService;
    private readonly AppSettings _appSettings;

    public GetQrCodeQueryHandler(IQrCodeService qrCodeService, IOptions<AppSettings> appSettings)
    {
        _qrCodeService = qrCodeService;
        _appSettings = appSettings.Value;
    }

    public async Task<byte[]> Handle(GetQrCodeQuery request, CancellationToken cancellationToken)
    {
        return await _qrCodeService.GenerateQrCodeAsync($"{_appSettings.BaseUrl}{request.ShortCode}");
    }
}
