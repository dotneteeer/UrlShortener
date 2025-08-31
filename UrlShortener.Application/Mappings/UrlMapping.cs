using AutoMapper;
using UrlShortener.Domain.Dtos;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Mappings;

public class UrlMapping : Profile
{
    public UrlMapping()
    {
        CreateMap<ShortenedUrl, ShortenedUrlDto>().ReverseMap();
    }
}