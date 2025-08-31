using UrlShortener.Domain.Interfaces.Entity;

namespace UrlShortener.Domain.Entities;

public class ShortenedUrl : IEntityId<long>, IAuditable
{
    public string OriginalUrl { get; set; }
    public string ShortUrl { get; set; }
    public int AccessCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public long Id { get; set; }
}