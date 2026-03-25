using AscentListerAPI.Data;
using AscentListerAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AscentListerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LocationController(AppDbContext context) : Controller
{
    [HttpGet]
    public async Task<ActionResult<List<Ascent>>> Get()
    {
        return await Task.FromResult(Ok(context.Ascents.ToList()));
    }
}