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
    
    [HttpPost("batch")]
    public async Task<ActionResult<List<Ascent>>> AddAscents(List<Ascent> ascents)
    {
        var newAscents = await service.AddAscentsAsync(ascents);
        return CreatedAtAction(nameof(GetAscents), newAscents);
    }
    
    [HttpPost]
    public async Task<ActionResult<Ascent>> AddAscent(Ascent ascent)
    {
        var newAscent = await service.AddAscentAsync(ascent);
        return CreatedAtAction(nameof(GetAscent), new { id = newAscent.AscentId }, newAscent);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<Ascent>> UpdateAscent(int id, Ascent ascent)
    {
        var updated = await service.UpdateAscentAsync(id, ascent);
        if (updated == null)
        {
            return NotFound($"No ascent found with id {id}");
        }
        return Ok();
    }
    
    [HttpDelete]
    public async Task<ActionResult<bool>> DeleteAscent(int id)
    {
        var deleted = await service.DeleteAscentAsync(id);
        if (!deleted)
        {
            return NotFound($"No ascent found with id {id}");
        }
        return Ok();
    }
}