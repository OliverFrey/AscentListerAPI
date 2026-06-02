using Route = AscentListerAPI.Models.Route;

namespace AscentListerAPI.Data.Repositories;

public interface IRouteRepository
{
    Task<Route?> GetByIdAsync(int id);
    Task AddAsync(Route route);
}
