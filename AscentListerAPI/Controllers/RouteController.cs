using AscentListerAPI.Models;
using AscentListerAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Route = AscentListerAPI.Models.Route;

namespace AscentListerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RouteController(RouteService routeService) : Controller
{
    [HttpGet]
    public async Task<ActionResult<List<Route>>> Get()
        =>await Task.FromResult(Ok(routeService.GetRoutesAsync()));
}