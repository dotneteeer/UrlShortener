using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Domain.Results;

namespace UrlShortener.Api.Controllers.Base;

/// <inheritdoc />
[Consumes(MediaTypeNames.Application.Json)]
[Route("")]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
[ApiController]
public class BaseController : ControllerBase
{
    /// <summary>
    ///     Handles the BaseResult of type T and returns the corresponding ActionResult
    /// </summary>
    /// <param name="result"></param>
    /// <typeparam name="T">Type of BaseResult</typeparam>
    /// <returns></returns>
    protected ActionResult<BaseResult<T>> HandleBaseResult<T>(BaseResult<T> result) where T : class
    {
        return result.IsSuccess switch
        {
            true => Ok(result),
            _ => BadRequest(result)
        };
    }
}