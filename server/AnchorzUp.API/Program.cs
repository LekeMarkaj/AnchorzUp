using AnchorzUp.Application.Interfaces;
using AnchorzUp.Infrastructure.Data;
using AnchorzUp.Infrastructure.Repositories;
using AnchorzUp.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using MediatR;
using System.Reflection;
using AnchorzUp.API.Middleware;
using AnchorzUp.Application.Configuration;
using AnchorzUp.Application.ShortUrl.Commands.CreateShortUrl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://localhost:3000", "http://localhost:52053", "https://localhost:52053")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Add Entity Framework
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AnchorzUpDbContext>(options =>
    options.UseSqlite(connectionString));

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateShortUrlCommand).Assembly));

// Add configuration
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Add repositories and services
builder.Services.AddScoped<IShortUrlRepository, ShortUrlRepository>();
builder.Services.AddScoped<IShortUrlService, ShortUrlService>();
builder.Services.AddScoped<IQrCodeService, QrCodeService>();

// Add middleware
builder.Services.AddScoped<ExceptionHandlingMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseCors("AllowReactApp");

// Add global exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Serve static files (React frontend)
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();
app.MapControllers();

// Fallback to React app for client-side routing
app.MapFallbackToFile("index.html");

// Ensure database is created and apply migrations
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AnchorzUpDbContext>();
    context.Database.Migrate();
}

app.Run();
