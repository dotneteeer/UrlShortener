using System.Reflection;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.OpenApi.Models;
using Serilog;

namespace UrlShortener.Api;

public static class Startup
{
    private const string AppStartupSectionName = "AppStartupSettings";
    private const string AppStartupUrlLogName = "AppStartupUrlLog";

    /// <summary>
    ///     Configures and adds Swagger documentation generation with JWT authentication support to the service collection.
    ///     Includes API versioning, security definitions, and XML documentation.
    /// </summary>
    /// <param name="services">The service collection to which Swagger services are added.</param>
    public static void AddSwagger(this IServiceCollection services)
    {
        const string apiVersion = "v1";

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(apiVersion, new OpenApiInfo
            {
                Version = "v1",
                Title = "UrlShortener.Api",
                Description = "UrlShortener api v1",
                //maybe add in future
                //TermsOfService = termsOfServiceUrl,
                Contact = new OpenApiContact
                {
                    Name = "UrlShortener api contact"
                    //maybe add in future
                    //Url = termsOfServiceUrl
                }
            });

            var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
        });
    }


    /// <summary>Logs all URLs on which the application is listening when it starts.</summary>
    /// <param name="app">The web application to which the middleware is added.</param>
    public static void LogListeningUrls(this WebApplication app)
    {
        app.Lifetime.ApplicationStarted.Register(() =>
        {
            var hosts = app.GetHosts().ToList();

            var appStartupHostLog =
                app.Configuration.GetSection(AppStartupSectionName).GetValue<string>(AppStartupUrlLogName);

            hosts.ForEach(host => Log.Information("{0}{1}", appStartupHostLog, host));
        });
    }

    /// <summary>
    ///     Configures structured logging for the application using Serilog.
    /// </summary>
    /// <param name="host">The host builder used to configure the application.</param>
    public static void AddLogging(this IHostBuilder host)
    {
        host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
    }

    /// <summary>
    ///     Configures the application to use localization with specified supported cultures and default request culture.
    /// </summary>
    /// <param name="app">The web application to which the localization middleware is added.</param>
    public static void UseLocalization(this IApplicationBuilder app)
    {
        app.UseRequestLocalization(options =>
        {
            string[] supportedCultures = ["en", "ru-by"];
            options.DefaultRequestCulture = new RequestCulture(supportedCultures[0]);
            options.AddSupportedCultures(supportedCultures);
            options.AddSupportedUICultures(supportedCultures);
            options.ApplyCurrentCultureToResponseHeaders = true;
        });
    }

    private static IEnumerable<string> GetHosts(this IApplicationBuilder app)
    {
        HashSet<string> hosts = [];

        var serverAddressesFeature = app.ServerFeatures.Get<IServerAddressesFeature>();
        serverAddressesFeature?.Addresses.ToList().ForEach(x => hosts.Add(x));

        return hosts;
    }
}