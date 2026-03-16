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
}