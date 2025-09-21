using MediatR;
using AnchorzUp.Application.Interfaces;

namespace AnchorzUp.Application.ShortUrl.Commands.DeleteShortUrl;

public class DeleteShortUrlCommandHandler : IRequestHandler<DeleteShortUrlCommand, Unit>
{
    private readonly IShortUrlService _shortUrlService;

    public DeleteShortUrlCommandHandler(IShortUrlService shortUrlService)
    {
        _shortUrlService = shortUrlService;
    }

    public async Task<Unit> Handle(DeleteShortUrlCommand request, CancellationToken cancellationToken)
    {
        await _shortUrlService.DeleteShortUrlAsync(request.Id);
        return Unit.Value;
    }
}
