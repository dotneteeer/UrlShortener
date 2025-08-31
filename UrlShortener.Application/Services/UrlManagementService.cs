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

public class UrlManagementService(
    IBaseRepository<ShortenedUrl> urlRepository,
    IMapper mapper,
    IOptions<BusinessRules> businessRules) : IUrlManagementService
{
    private readonly BusinessRules _businessRules = businessRules.Value;

    public Task<QueryableResult<ShortenedUrl>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var urls = urlRepository.GetAll();

        return Task.FromResult(QueryableResult<ShortenedUrl>.Success(urls));
    }

    public async Task<BaseResult<ShortenedUrlDto>> EditAsync(ShortenedUrlDto dto,
        CancellationToken cancellationToken = default)
    {
        var url = await urlRepository.GetAll().FirstOrDefaultAsync(x => x.Id == dto.Id, cancellationToken);
        if (url == null)
            return BaseResult<ShortenedUrlDto>.Failure(ErrorMessage.UrlNotFound, (int)ErrorCodes.UrlNotFound);

        if (!dto.OriginalUrl.IsUrl() || dto.ShortUrl.Length != _businessRules.ShortenedUrlLength)
            return BaseResult<ShortenedUrlDto>.Failure(ErrorMessage.InvalidUrl, (int)ErrorCodes.InvalidUrl);

        mapper.Map(dto, url);

        urlRepository.Update(url);
        await urlRepository.SaveChangesAsync(cancellationToken);

        return BaseResult<ShortenedUrlDto>.Success(mapper.Map<ShortenedUrlDto>(url));
    }

    public async Task<BaseResult<ShortenedUrlDto>> DeleteAsync(DeleteShortenedUrlDto dto,
        CancellationToken cancellationToken = default)
    {
        var url = await urlRepository.GetAll().FirstOrDefaultAsync(x => x.Id == dto.Id, cancellationToken);
        if (url == null)
            return BaseResult<ShortenedUrlDto>.Failure(ErrorMessage.UrlNotFound, (int)ErrorCodes.UrlNotFound);

        urlRepository.Remove(url);
        await urlRepository.SaveChangesAsync(cancellationToken);

        return BaseResult<ShortenedUrlDto>.Success(mapper.Map<ShortenedUrlDto>(url));
    }
}