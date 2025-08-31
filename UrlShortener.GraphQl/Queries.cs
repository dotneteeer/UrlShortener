using UrlShortener.Domain.Entities;
using UrlShortener.Domain.Interfaces.Services;
using UrlShortener.GraphQl.Helpers;
using UrlShortener.GraphQl.Middlewares;

namespace UrlShortener.GraphQl;

public class Queries
{
    [GraphQLDescription("Gets all shortened URLs")]
    [UseOffsetPagingValidationMiddleware]
    [UseOffsetPaging]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<ShortenedUrl>> GetUrls([Service] IUrlManagementService urlService,
        CancellationToken cancellationToken)
    {
        var result = await urlService.GetAllAsync(cancellationToken);

        if (!result.IsSuccess)
            throw GraphQlExceptionHelper.GetException(result.ErrorMessage!);

        return result.Data;
    }
}