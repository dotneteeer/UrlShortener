using GraphQL.Server.Ui.Voyager;
using HotChocolate.Types.Pagination;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using UrlShortener.Application.Settings;
using UrlShortener.GraphQl.ErrorFilters;
using UrlShortener.GraphQl.Types;

namespace UrlShortener.GraphQl.DependencyInjection;

public static class DependencyInjection
{
    private const string GraphQlEndpoint = "/graphql/endpoint";
    private const string GraphQlVoyagerEndpoint = "/graphql-voyager";

    /// <summary>
    ///     Adds GraphQl services
    /// </summary>
    /// <param name="services"></param>
    public static void AddGraphQl(this IServiceCollection services)
    {
        services.AddGraphQLServer()
            .AddQueryType<Queries>()
            .AddType<ShortenedUrlType>()
            .AddTypeExtension<CollectionSegmentInfoType>()
            .AddSorting()
            .AddFiltering()
            .AddErrorFilter<PublicErrorFilter>()
            .ModifyPagingOptions(opt =>
            {
                using var provider = services.BuildServiceProvider();
                using var scope = provider.CreateScope();
                var defaultSize = scope.ServiceProvider.GetRequiredService<IOptions<PaginationRules>>().Value
                    .DefaultPageSize;

                opt.DefaultPageSize = defaultSize;
                opt.IncludeTotalCount = true;
            })
            .ModifyCostOptions(opt => opt.MaxFieldCost *= 2);
    }

    /// <summary>
    ///     Enables the use of GraphQl services
    /// </summary>
    /// <param name="app"></param>
    public static void UseGraphQl(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
            app.UseGraphQLVoyager(GraphQlVoyagerEndpoint, new VoyagerOptions { GraphQLEndPoint = GraphQlEndpoint });

        app.MapGraphQL(GraphQlEndpoint);
    }
}