using AscentListerAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AscentListerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LocationController : Controller
{
    static List<Location> _locations = new List<Location>
    {
        new Location { LocationId = 1, LocationName = "Unteres Lehn", LocationAreaName = "Lehn", locationCountry = "Schweiz"}
    };
    
    [HttpGet]
    public async Task<ActionResult<List<Ascent>>> Get()
        =>await Task.FromResult(Ok(_locations));
}