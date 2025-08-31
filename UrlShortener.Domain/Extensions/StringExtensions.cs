namespace UrlShortener.Domain.Extensions;

public static class StringExtensions
{
    public static bool IsUrl(this string? url)
    {
        return Uri.TryCreate(url?.Trim(), UriKind.Absolute, out var uri)
               && (uri.Scheme == Uri.UriSchemeHttp ||
                   uri.Scheme == Uri.UriSchemeHttps);
    }
}