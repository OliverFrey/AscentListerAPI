using AscentListerAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AscentListerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LocationController : Controller
{
    [HttpGet]
    public async Task<ActionResult<List<Ascent>>> Get()
        =>await Task.FromResult(Ok(_locations));
}