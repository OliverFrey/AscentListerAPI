using AscentListerAPI.Data;
using Microsoft.EntityFrameworkCore;
using Route = AscentListerAPI.Models.Route;

namespace AscentListerAPI.Services;

public class RouteService(AppDbContext context)
{
    public async Task<List<Route>> GetRoutesAsync()
    {
        var routes = await context.Routes.ToListAsync();
        return routes;
    } 
}