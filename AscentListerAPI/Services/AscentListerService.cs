using AscentListerAPI.Data;
using AscentListerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Route = AscentListerAPI.Models.Route;

namespace AscentListerAPI.Services;

public class AscentListerService(AppDbContext context) : IAscentListerService
{
    public async Task<List<Ascent>> GetAllAscentsAsync()
        => await context.Ascents.AsNoTracking()
            .Include(r => r.Route)
                .ThenInclude(r => r.Location)
            .ToListAsync();
    
    public async Task<List<Ascent>> AddAscentsAsync(List<Ascent> ascents)
    {
        try
        {
            foreach (var ascent in ascents)
            {
                var location = await AddLocationAsync(ascent.Route.Location);
                ascent.Route.Location = location;
                var route = await AddRouteAsync(ascent.Route);
                ascent.Route = route;
            }

            await context.Ascents.AddRangeAsync(ascents);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return ascents;
    }

    private async Task<Location> AddLocationAsync(Location location)
    {
        var existingLocation = await context.Locations.FindAsync(location.LocationId);
        if (existingLocation != null) return existingLocation;
        context.Locations.Add(location);
        return location;
    }

    private async Task<Route> AddRouteAsync(Route route)
    {
        var existingRoute = await context.Routes.FindAsync(route.RouteId);
        if (existingRoute != null) return existingRoute;
        context.Routes.Add(route);
        return route;
    }
}