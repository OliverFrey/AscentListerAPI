using AscentListerAPI.Models;

namespace AscentListerAPI.Data.Repositories;

public interface ILocationRepository
{
    Task<Location?> GetByIdAsync(int id);
    Task AddAsync(Location location);
}
