using System.ComponentModel.DataAnnotations;

namespace AnchorzUp.Domain.Entities;

public class Click
{
    public Guid Id { get; set; }
    
    public Guid ShortUrlId { get; set; }
    
    public DateTime ClickedAt { get; set; } = DateTime.UtcNow;
    
    [MaxLength(45)]
    public string? IpAddress { get; set; }
    
    [MaxLength(500)]
    public string? UserAgent { get; set; }
    
    [MaxLength(100)]
    public string? Referer { get; set; }
    
    // Navigation property
    public ShortUrl ShortUrl { get; set; } = null!;
}
