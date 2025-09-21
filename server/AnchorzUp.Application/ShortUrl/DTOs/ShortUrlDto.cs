namespace AnchorzUp.Application.ShortUrl.DTOs;

public class ShortUrlDto
{
    public Guid Id { get; set; }
    public string OriginalUrl { get; set; } = string.Empty;
    public string ShortUrl { get; set; } = string.Empty;
    public string ShortCode { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public int ClickCount { get; set; }
    public DateTime? LastAccessedAt { get; set; }
    public string QrCodeBase64 { get; set; } = string.Empty;
}

public class CreateShortUrlDto
{
    public string OriginalUrl { get; set; } = string.Empty;
    public DateTime? ExpiresAt { get; set; }
}

public class CreateShortUrlResponseDto
{
    public Guid Id { get; set; }
    public string OriginalUrl { get; set; } = string.Empty;
    public string ShortUrl { get; set; } = string.Empty;
    public string ShortCode { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public int ClickCount { get; set; }
    public string QrCodeBase64 { get; set; } = string.Empty;
}
