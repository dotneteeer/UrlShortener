using System.Security.Cryptography;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UrlShortener.Application.Enums;
using UrlShortener.Application.Resources;
using UrlShortener.Application.Settings;
using UrlShortener.Domain.Dtos;
using UrlShortener.Domain.Entities;
using UrlShortener.Domain.Extensions;
using UrlShortener.Domain.Interfaces.Repository;
using UrlShortener.Domain.Interfaces.Services;
using UrlShortener.Domain.Results;

namespace UrlShortener.Application.Services;

public class UrlService(
    IBaseRepository<ShortenedUrl> urlRepository,
    IOptions<BusinessRules> businessRules,
    IMapper mapper)
    : IUrlService
{
    private readonly BusinessRules _businessRules = businessRules.Value;

    public async Task<BaseResult<ShortenedUrlDto>> ShortenUrlAsync(CreateShortenedUrlDto dto,
        CancellationToken cancellationToken = default)
    {
        if (!dto.OriginalUrl.IsUrl())
            return BaseResult<ShortenedUrlDto>.Failure(ErrorMessage.InvalidUrl, (int)ErrorCodes.InvalidUrl);

        var url = await urlRepository.GetAll()
            .FirstOrDefaultAsync(x => x.OriginalUrl == dto.OriginalUrl, cancellationToken);
        if (url != null)
            return BaseResult<ShortenedUrlDto>.Failure(ErrorMessage.UrlAlreadyExists, (int)ErrorCodes.UrlAlreadyExists);

        var existingShortUrl = await urlRepository.GetAll().Select(x => x.ShortUrl).ToHashSetAsync(cancellationToken);

        url = new ShortenedUrl
        {
            OriginalUrl = dto.OriginalUrl,
            ShortUrl = GenerateRandomString(_businessRules.ShortenedUrlLength, existingShortUrl)
        };

        await urlRepository.CreateAsync(url, cancellationToken);
        await urlRepository.SaveChangesAsync(cancellationToken);

        var urlDto = mapper.Map<ShortenedUrlDto>(url);
        return BaseResult<ShortenedUrlDto>.Success(urlDto);
    }

    public async Task<BaseResult<ShortenedUrlDto>> GetOriginalUrlAsync(GetShortenedUrlDto dto,
        CancellationToken cancellationToken = default)
    {
        var url = await urlRepository.GetAll()
            .FirstOrDefaultAsync(x => x.ShortUrl == dto.ShortenedUrl, cancellationToken);
        if (url == null)
            return BaseResult<ShortenedUrlDto>.Failure(ErrorMessage.UrlNotFound, (int)ErrorCodes.UrlNotFound);

        url.AccessCount++;
        await urlRepository.SaveChangesAsync(cancellationToken);

        var urlDto = mapper.Map<ShortenedUrlDto>(url);
        return BaseResult<ShortenedUrlDto>.Success(urlDto);
    }

    private static string GenerateRandomString(int length, HashSet<string> existingStrings)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        string str;
        var charsStr = new char[length];
        do
        {
            for (var i = 0; i < length; i++) charsStr[i] = chars[RandomNumberGenerator.GetInt32(chars.Length)];
            str = new string(charsStr);
        } while (existingStrings.Contains(str));

        return str;
    }
}