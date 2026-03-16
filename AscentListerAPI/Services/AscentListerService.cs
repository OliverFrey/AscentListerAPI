using AscentListerAPI.Models;
using Route = AscentListerAPI.Models.Route;

namespace AscentListerAPI.Services;

public class AscentListerService : IAscentListerService
{
    static List<Location> _locations = new List<Location>
    {
        new Location { LocationId = 1, LocationName = "Unteres Lehn", LocationAreaName = "Lehn", locationCountry = "Schweiz", LocationStatus = StatusEnum.EXISTS}
    };
    
    static List<Route> _routes = new List<Route>
    {
        new Route() { RouteId = 1, RouteName = "Das ist eine Route", Grade = "6a", LocationId = 1, RouteStatus = StatusEnum.EXISTS },
        new Route() { RouteId = 2, RouteName = "Route 2", Grade = "7a", LocationId = 1, RouteStatus = StatusEnum.EXISTS },
        new Route() { RouteId = 3, RouteName = "Route 3", Grade = "6a+", LocationId = 1, RouteStatus = StatusEnum.EXISTS }
    };
    
    static List<Ascent> _ascents = new List<Ascent>
    {
        new Ascent { AscentId = 1, RouteId = 1, Date = new DateOnly(2026,03,16), Attempts = 1, Style = "o", Comments = "", Status = StatusEnum.EXISTS},
        new Ascent { AscentId = 2, RouteId = 2, Date = new DateOnly(2026,03,16), Attempts = 2, Style = "rp", Comments = "", Status = StatusEnum.EXISTS},
        new Ascent { AscentId = 3, RouteId = 3, Date = new DateOnly(2026,03,16), Attempts = 1, Style = "o", Comments = "", Status = StatusEnum.EXISTS}
    };
    
    
    public async Task<List<Ascent>> GetAllAscentsAsync()
        => await Task.FromResult(_ascents);

    public Task<Ascent> GetAscentByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Ascent> AddAscentAsync(Ascent ascent)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAscentAsync(int id, Ascent ascent)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAscentAsync(int id)
    {
        throw new NotImplementedException();
    }
}