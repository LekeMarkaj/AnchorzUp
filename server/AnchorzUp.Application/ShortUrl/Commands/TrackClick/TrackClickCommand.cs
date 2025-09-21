using MediatR;
using AnchorzUp.Application.ShortUrl.DTOs;

namespace AnchorzUp.Application.ShortUrl.Commands.TrackClick;

public class TrackClickCommand : IRequest<ShortUrlDto>
{
    public string ShortCode { get; set; } = string.Empty;
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? Referer { get; set; }
}
