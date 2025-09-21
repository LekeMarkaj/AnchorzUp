using MediatR;
using AnchorzUp.Application.ShortUrl.DTOs;

namespace AnchorzUp.Application.ShortUrl.Queries.GetAllShortUrls;

public class GetAllShortUrlsQuery : IRequest<IEnumerable<ShortUrlDto>>
{
}
