using AscentListerAPI.Models;
using AscentListerAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AscentListerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AscentController(IAscentListerService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Ascent>>> GetAscents()
        => Ok(await service.GetAllAscentsAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<Ascent>> GetAscent(int id)
    {
        var ascent = await service.GetAscentByIdAsync(id);
        if (ascent == null)
        {
            return NotFound($"No ascent found with id {id}");
        }
        return Ok(ascent);
    }
}