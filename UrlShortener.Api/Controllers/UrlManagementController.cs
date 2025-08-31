using Microsoft.AspNetCore.Mvc;
using UrlShortener.Api.Controllers.Base;
using UrlShortener.Domain.Dtos;
using UrlShortener.Domain.Interfaces.Services;
using UrlShortener.Domain.Results;

namespace UrlShortener.Api.Controllers;

public class UrlManagementController(IUrlManagementService urlService) : BaseController
{
    /// <summary>
    ///     Updates the details of an existing shortened URL.
    /// </summary>
    /// <param name="dto">The data transfer object containing the updated details of the shortened URL.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A task representing the asynchronous operation, containing the result of the update operation as a BaseResult
    ///     of ShortenedUrlDto.
    /// </returns>
    [HttpPut]
    public async Task<ActionResult<BaseResult<ShortenedUrlDto>>> EditUrl([FromBody] ShortenedUrlDto dto,
        CancellationToken cancellationToken)
    {
        var result = await urlService.EditAsync(dto, cancellationToken);

        return HandleBaseResult(result);
    }

    /// <summary>
    ///     Deletes an existing shortened URL.
    /// </summary>
    /// <param name="dto">The data transfer object containing the details of the shortened URL to be deleted.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A task representing the asynchronous operation, containing the result of the delete operation as a BaseResult
    ///     of ShortenedUrlDto.
    /// </returns>
    [HttpDelete("{id:long}")]
    public async Task<ActionResult<BaseResult<ShortenedUrlDto>>> DeleteUrl(long id,
        CancellationToken cancellationToken)
    {
        var dto = new DeleteShortenedUrlDto(id);
        var result = await urlService.DeleteAsync(dto, cancellationToken);

        return HandleBaseResult(result);
    }
}