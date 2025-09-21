using MediatR;

namespace AnchorzUp.Application.ShortUrl.Queries.GetQrCode;

public class GetQrCodeQuery : IRequest<byte[]>
{
    public string ShortCode { get; set; } = string.Empty;
}
