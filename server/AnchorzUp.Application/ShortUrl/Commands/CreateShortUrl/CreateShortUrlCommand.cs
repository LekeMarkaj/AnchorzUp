using MediatR;
using AnchorzUp.Application.ShortUrl.DTOs;

namespace AnchorzUp.Application.ShortUrl.Commands.CreateShortUrl;

public class CreateShortUrlCommand : IRequest<CreateShortUrlResponseDto>
{
    public string OriginalUrl { get; set; } = string.Empty;
    public DateTime? ExpiresAt { get; set; }
}
