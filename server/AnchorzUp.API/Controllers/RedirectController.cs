using AnchorzUp.Application.ShortUrl.Commands.TrackClick;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AnchorzUp.API.Controllers;

[ApiController]
[Route("")]
public class RedirectController : ControllerBase
{
    private readonly IMediator _mediator;

    public RedirectController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{shortCode}")]
    public async Task<IActionResult> RedirectToOriginalUrl(string shortCode)
    {
        try
        {
            var command = new TrackClickCommand
            {
                ShortCode = shortCode,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                UserAgent = HttpContext.Request.Headers.UserAgent.ToString(),
                Referer = HttpContext.Request.Headers.Referer.ToString()
            };

            var result = await _mediator.Send(command);
            return Redirect(result.OriginalUrl);
        }
        catch (ArgumentException)
        {
            return NotFound(new { message = "Short URL not found" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An error occurred while redirecting" });
        }
    }
}
