using System.ComponentModel.DataAnnotations;

namespace AnchorzUp.Domain.Entities;

public class ShortUrl
{
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(2048)]
    public string OriginalUrl { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(10)]
    public string ShortCode { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? ExpiresAt { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public int ClickCount { get; set; } = 0;
    
    public DateTime? LastAccessedAt { get; set; }
    
    // Navigation properties
    public ICollection<Click> Clicks { get; set; } = new List<Click>();
}
