using AscentListerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AscentListerAPI.Data.Repositories;

public class AscentRepository(AppDbContext context) : IAscentRepository
{
    public async Task<List<Ascent>> GetAllWithGraphAsync() =>
        await context.Ascents.AsNoTracking()
            .Include(a => a.Route)
                .ThenInclude(r => r.Location)
            .ToListAsync();

    public async Task AddRangeAsync(IEnumerable<Ascent> ascents) =>
        await context.Ascents.AddRangeAsync(ascents);

    public Task<int> SaveChangesAsync() => context.SaveChangesAsync();
}
