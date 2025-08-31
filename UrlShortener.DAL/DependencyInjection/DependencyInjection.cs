using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.DAL.Repositories;
using UrlShortener.Domain.Entities;
using UrlShortener.Domain.Interfaces.Repository;

namespace UrlShortener.DAL.DependencyInjection;

public static class DependencyInjection
{
    public static void AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MariaDb");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        services.InitRepositories();
    }

    /// <summary>
    ///     Migrates the database
    /// </summary>
    /// <param name="services"></param>
    public static async Task MigrateDatabaseAsync(this IServiceCollection services)
    {
        var dbContext = services.BuildServiceProvider().GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    private static void InitRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBaseRepository<ShortenedUrl>, BaseRepository<ShortenedUrl>>();
    }
}