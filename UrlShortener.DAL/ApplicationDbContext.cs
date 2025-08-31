using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UrlShortener.Application.Settings;
using UrlShortener.DAL.Interceptors;
using UrlShortener.Domain.Entities;
using ILogger = Serilog.ILogger;

namespace UrlShortener.DAL;

public sealed class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    ILogger logger,
    IOptions<BusinessRules> businessRules)
    : DbContext(options)
{
    private readonly BusinessRules _businessRules = businessRules.Value;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(logger.Information, LogLevel.Information);
        optionsBuilder.AddInterceptors(new DateInterceptor());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.Entity<ShortenedUrl>()
            .Property(x => x.ShortUrl).HasMaxLength(_businessRules.ShortenedUrlLength).IsRequired();
    }
}