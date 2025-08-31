using Microsoft.AspNetCore.Mvc;
using UrlShortener.Api.Controllers.Base;
using UrlShortener.Domain.Dtos;
using UrlShortener.Domain.Interfaces.Services;
using UrlShortener.Domain.Results;

namespace UrlShortener.Api.Controllers;

public class UrlShortenerController(IUrlService urlService) : BaseController
{
    /// <summary>
    ///     Handles the creation of a shortened URL based on the provided original URL.
    /// </summary>
    /// <param name="createShortenedUrlDto">The DTO containing the original URL to be shortened.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a <see cref="BaseResult{T}" />
    ///     with a <see cref="ShortenedUrlDto" /> that includes both the original and shortened URLs.
    /// </returns>
    [HttpPost("shorten")]
    public async Task<ActionResult<BaseResult<ShortenedUrlDto>>> ShortenUrl(
        [FromBody] CreateShortenedUrlDto createShortenedUrlDto, CancellationToken cancellationToken)
    {
        var result = await urlService.ShortenUrlAsync(createShortenedUrlDto, cancellationToken);

        return HandleBaseResult(result);
    }

    /// <summary>
    ///     Redirects the user to the original URL associated with the provided shortened URL.
    /// </summary>
    /// <param name="shortenedUrl">The shortened URL to be resolved to its original URL.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains an <see cref="ActionResult{T}" /> where
    ///     <see cref="BaseResult{ShortenedUrlDto}" /> includes the original URL and shortened URL, or an appropriate error
    ///     message if the operation fails.
    /// </returns>
    [HttpGet("~/{shortenedUrl}")] // To make the link short, we use "~/" to override the controller route
    public async Task<ActionResult<BaseResult<ShortenedUrlDto>>> RedirectToOriginalUrl(string shortenedUrl,
        CancellationToken cancellationToken)
    {
        var result = await urlService.GetOriginalUrlAsync(new GetShortenedUrlDto(shortenedUrl), cancellationToken);

        if (!result.IsSuccess) return HandleBaseResult(result);

        return Redirect(result.Data.OriginalUrl);
    }
}