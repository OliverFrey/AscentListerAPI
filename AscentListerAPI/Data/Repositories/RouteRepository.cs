using Route = AscentListerAPI.Models.Route;

namespace AscentListerAPI.Data.Repositories;

public class RouteRepository(AppDbContext context) : IRouteRepository
{
    public async Task<Route?> GetByIdAsync(int id) =>
        await context.Routes.FindAsync(id);

    public async Task AddAsync(Route route) =>
        await context.Routes.AddAsync(route);
}
