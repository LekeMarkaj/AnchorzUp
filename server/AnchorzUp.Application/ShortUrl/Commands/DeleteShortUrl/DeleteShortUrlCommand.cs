using MediatR;

namespace AnchorzUp.Application.ShortUrl.Commands.DeleteShortUrl;

public class DeleteShortUrlCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}
