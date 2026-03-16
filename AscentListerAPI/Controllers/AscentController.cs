using AscentListerAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AscentListerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AscentController : ControllerBase
{
    static List<Ascent> _ascents = new List<Ascent>
    {
        new Ascent { AscentId = 1, RouteId = 1, Date = new DateOnly(2026,03,16), Attempts = 1, Style = "o", Comments = ""},
        new Ascent { AscentId = 2, RouteId = 2, Date = new DateOnly(2026,03,16), Attempts = 2, Style = "rp", Comments = ""},
        new Ascent { AscentId = 3, RouteId = 3, Date = new DateOnly(2026,03,16), Attempts = 1, Style = "o", Comments = ""}
    };
    
    [HttpGet]
    public async Task<ActionResult<List<Ascent>>> Get()
        =>await Task.FromResult(Ok(_ascents));
}