using AscentListerAPI.Models;

namespace AscentListerAPI.Data.Repositories;

public class LocationRepository(AppDbContext context) : ILocationRepository
{
    public async Task<Location?> GetByIdAsync(int id) =>
        await context.Locations.FindAsync(id);

    public async Task AddAsync(Location location) =>
        await context.Locations.AddAsync(location);
}
