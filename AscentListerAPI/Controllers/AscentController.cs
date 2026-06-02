using AscentListerAPI.Models;
using AscentListerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AscentListerAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AscentController(IAscentListerService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Ascent>>> GetAscents()
    {
        var ascents = await service.GetAllAscentsAsync();
        return Ok(ascents);
    }
    
    [HttpPost("batch")]
    public async Task<ActionResult<List<Ascent>>> AddAscents(List<Ascent> ascents)
    {
        var newAscents = await service.AddAscentsAsync(ascents);
        return CreatedAtAction(nameof(GetAscents), newAscents);
    }
}