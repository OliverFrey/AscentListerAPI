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

    public async Task<Ascent?> GetAscentByIdAsync(int id)
    {
        var ascent = await context.Ascents
                .AsNoTracking()
                .Include(a => a.Route)
                    .ThenInclude(r => r.Location)
                .Where(a => a.AscentId == id).FirstOrDefaultAsync();
        return ascent;
    }

    public async Task<Ascent> AddAscentAsync(Ascent ascent)
    {
        await AddLocationAsync(ascent.Route.Location);
        await AddRouteAsync(ascent.Route);
        
        await context.Ascents.AddAsync(ascent);
        await context.SaveChangesAsync();
        return ascent;
    }
    
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

    public async Task<Ascent?> UpdateAscentAsync(int id, Ascent ascent)
    {
        if (id != ascent.AscentId)
            return null;
        
        var existingAscent = await context.Ascents.FindAsync(id);
        if (existingAscent == null)
            return null;
        
        context.Ascents.Update(ascent);
        await context.SaveChangesAsync();
        return ascent;
    }

    public async Task<bool> DeleteAscentAsync(int id)
    {
        var ascent = await context.Ascents.FindAsync(id);
        if (ascent == null)
            return false;
        
        context.Ascents.Remove(ascent);
        await context.SaveChangesAsync();
        return true;
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