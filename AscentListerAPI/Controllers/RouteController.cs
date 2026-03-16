using AscentListerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Route = AscentListerAPI.Models.Route;

namespace AscentListerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RouteController : Controller
{
    [HttpGet]
    public async Task<ActionResult<List<Route>>> Get()
        =>await Task.FromResult(Ok(_routes));
}