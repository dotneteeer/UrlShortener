using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Application.Services;
using UrlShortener.Domain.Interfaces.Services;

namespace UrlShortener.Application.DependencyInjection;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.InitServices();
    }

    private static void InitServices(this IServiceCollection services)
    {
        services.AddScoped<IUrlService, UrlService>();
    }
}