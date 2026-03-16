using Microsoft.AspNetCore.Mvc;
using Route = AscentListerAPI.Models.Route;

namespace AscentListerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RouteController : Controller
{
    static List<Route> _routes = new List<Route>
    {
        new Route()
        {
            RouteId = 1, RouteName = "Das ist eine Route", Grade = "6a",
            LocationId = 1
        },
        new Route()
        {
            RouteId = 2, RouteName = "Route 2", Grade = "7a",
            LocationId = 1
        },
        new Route()
        {
            RouteId = 3, RouteName = "Route 3", Grade = "6a+",
            LocationId = 1
        }
    };
    
    [HttpGet]
    public async Task<ActionResult<List<Route>>> Get()
        =>await Task.FromResult(Ok(_routes));
}