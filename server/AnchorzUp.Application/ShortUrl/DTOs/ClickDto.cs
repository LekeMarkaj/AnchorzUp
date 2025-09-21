namespace AnchorzUp.Application.ShortUrl.DTOs;

public class ClickDto
{
    public Guid Id { get; set; }
    public Guid ShortUrlId { get; set; }
    public DateTime ClickedAt { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? Referer { get; set; }
}
