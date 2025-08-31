using UrlShortener.Domain.Dtos;
using UrlShortener.Domain.Entities;
using UrlShortener.Domain.Results;

namespace UrlShortener.Domain.Interfaces.Services;

public interface IUrlManagementService
{
    public Task<QueryableResult<ShortenedUrl>> GetAllAsync(CancellationToken cancellationToken = default);

    public Task<BaseResult<ShortenedUrlDto>> EditAsync(ShortenedUrlDto dto,
        CancellationToken cancellationToken = default);

    public Task<BaseResult<ShortenedUrlDto>> DeleteAsync(DeleteShortenedUrlDto dto,
        CancellationToken cancellationToken = default);
}