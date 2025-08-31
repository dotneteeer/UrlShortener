using UrlShortener.Domain.Entities;

namespace UrlShortener.GraphQl.Types;

public class ShortenedUrlType : ObjectType<ShortenedUrl>
{
    protected override void Configure(IObjectTypeDescriptor<ShortenedUrl> descriptor)
    {
        descriptor.Description("Represents a shortened URL entity.");
        descriptor.Field(f => f.Id).Description("The unique identifier of the shortened URL.");
        descriptor.Field(f => f.OriginalUrl).Description("The original URL that was shortened.");
        descriptor.Field(f => f.ShortUrl).Description("The shortened URL.");
        descriptor.Field(f => f.CreatedAt).Description("The date and time when the URL was shortened.");
        descriptor.Field(f => f.AccessCount).Description("The number of times the shortened URL has been accessed.");
    }
}