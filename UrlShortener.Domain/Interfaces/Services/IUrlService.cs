using UrlShortener.Domain.Dtos;
using UrlShortener.Domain.Results;

namespace UrlShortener.Domain.Interfaces.Services;

public interface IUrlService
{
    /// <summary>
    ///     Asynchronously generates a shortened URL for a given original URL.
    /// </summary>
    /// <param name="dto">The data transfer object containing the original URL to be shortened.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a
    ///     <see cref="BaseResult{ShortenedUrlDto}" />
    ///     with the original URL and its corresponding shortened URL if the operation is successful,
    ///     or an error message if the operation fails.
    /// </returns>
    Task<BaseResult<ShortenedUrlDto>> ShortenUrlAsync(CreateShortenedUrlDto dto,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously retrieves the original URL associated with a given shortened URL.
    /// </summary>
    /// <param name="dto">The data transfer object containing the shortened URL.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a
    ///     <see cref="BaseResult{ShortenedUrlDto}" />
    ///     with the original URL and the corresponding shortened URL if the operation is successful,
    ///     or an error message if the lookup fails.
    /// </returns>
    Task<BaseResult<ShortenedUrlDto>> GetOriginalUrlAsync(GetShortenedUrlDto dto,
        CancellationToken cancellationToken = default);
}