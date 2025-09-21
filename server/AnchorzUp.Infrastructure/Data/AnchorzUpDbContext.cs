using AnchorzUp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AnchorzUp.Infrastructure.Data;

public class AnchorzUpDbContext : DbContext
{
    public AnchorzUpDbContext(DbContextOptions<AnchorzUpDbContext> options) : base(options)
    {
    }

    public DbSet<ShortUrl> ShortUrls { get; set; }
    public DbSet<Click> Clicks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure ShortUrl entity
        modelBuilder.Entity<ShortUrl>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OriginalUrl).IsRequired().HasMaxLength(2048);
            entity.Property(e => e.ShortCode).IsRequired().HasMaxLength(10);
            entity.HasIndex(e => e.ShortCode).IsUnique();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.ClickCount).IsRequired().HasDefaultValue(0);
        });

        // Configure Click entity
        modelBuilder.Entity<Click>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ShortUrlId).IsRequired();
            entity.Property(e => e.ClickedAt).IsRequired();
            entity.Property(e => e.IpAddress).HasMaxLength(45);
            entity.Property(e => e.UserAgent).HasMaxLength(500);
            entity.Property(e => e.Referer).HasMaxLength(100);

            entity.HasOne(e => e.ShortUrl)
                  .WithMany(e => e.Clicks)
                  .HasForeignKey(e => e.ShortUrlId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
