namespace UrlShortener.Domain.Dtos;

public record ShortenedUrlDto(long Id, string OriginalUrl, string ShortUrl);