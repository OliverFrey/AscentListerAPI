using AscentListerAPI.Models;
using AscentListerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AscentListerAPI.Controllers;

/// <summary>
/// Endpoints for reading and recording climbing ascents. All actions require a
/// valid JWT Bearer token issued by the configured authentication provider.
/// </summary>
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AscentController(IAscentListerService service) : ControllerBase
{
    /// <summary>
    /// Returns every recorded ascent, including its associated route and location.
    /// </summary>
    /// <returns>The full list of ascents with their nested route and location data.</returns>
    /// <response code="200">The list of ascents was returned successfully.</response>
    /// <response code="401">The request was missing or carried an invalid Bearer token.</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<Ascent>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<Ascent>>> GetAscents()
    {
        var ascents = await service.GetAllAscentsAsync();
        return Ok(ascents);
    }

    /// <summary>
    /// Records a batch of ascents in a single request. Locations and routes that
    /// already exist are reused; missing ones are created automatically.
    /// </summary>
    /// <param name="ascents">The ascents to record, each carrying its route and location.</param>
    /// <returns>The persisted ascents, including any newly created routes and locations.</returns>
    /// <response code="200">The batch was recorded successfully.</response>
    /// <response code="400">The request body was missing or malformed.</response>
    /// <response code="401">The request was missing or carried an invalid Bearer token.</response>
    [HttpPost("batch")]
    [ProducesResponseType(typeof(List<Ascent>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<Ascent>>> AddAscents(List<Ascent> ascents)
    {
        var newAscents = await service.AddAscentsAsync(ascents);
        return Ok(newAscents);
    }
}
