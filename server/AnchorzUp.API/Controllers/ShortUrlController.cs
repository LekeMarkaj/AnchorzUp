using MediatR;
using Microsoft.AspNetCore.Mvc;
using AnchorzUp.Application.ShortUrl.Commands.CreateShortUrl;
using AnchorzUp.Application.ShortUrl.Queries.GetAllShortUrls;
using AnchorzUp.Application.ShortUrl.Commands.DeleteShortUrl;
using AnchorzUp.Application.ShortUrl.Queries.GetQrCode;
using AnchorzUp.Application.ShortUrl.DTOs;

namespace AnchorzUp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShortUrlController : ControllerBase
{
    private readonly IMediator _mediator;

    public ShortUrlController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<ActionResult<CreateShortUrlResponseDto>> CreateShortUrl([FromBody] CreateShortUrlDto request)
    {
        try
        {
            var command = new CreateShortUrlCommand
            {
                OriginalUrl = request.OriginalUrl,
                ExpiresAt = request.ExpiresAt
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the short URL" });
        }
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShortUrlDto>>> GetAllShortUrls()
    {
        try
        {
            var query = new GetAllShortUrlsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving short URLs" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteShortUrl(Guid id)
    {
        try
        {
            var command = new DeleteShortUrlCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the short URL" });
        }
    }

    [HttpGet("qr/{shortCode}")]
    public async Task<ActionResult> GetQrCode(string shortCode)
    {
        try
        {
            var query = new GetQrCodeQuery { ShortCode = shortCode };
            var qrCodeBytes = await _mediator.Send(query);
            return File(qrCodeBytes, "image/png");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while generating QR code" });
        }
    }
}
