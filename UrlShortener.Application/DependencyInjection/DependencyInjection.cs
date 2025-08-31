using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Application.Mappings;
using UrlShortener.Application.Services;
using UrlShortener.Application.Validators;
using UrlShortener.Domain.Dtos.Page;
using UrlShortener.Domain.Interfaces.Services;
using UrlShortener.Domain.Interfaces.Validation;

namespace UrlShortener.Application.DependencyInjection;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(UrlMapping));
        services.InitServices();
    }

    private static void InitServices(this IServiceCollection services)
    {
        services.AddScoped<IUrlService, UrlService>();
        services.AddScoped<IUrlManagementService, UrlManagementService>();

        services.AddScoped<INullSafeValidator<OffsetPageDto>, OffsetPageDtoValidator>();
    }
}