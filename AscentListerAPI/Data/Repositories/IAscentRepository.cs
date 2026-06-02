using AscentListerAPI.Models;

namespace AscentListerAPI.Data.Repositories;

public interface IAscentRepository
{
    Task<List<Ascent>> GetAllWithGraphAsync();
    Task AddRangeAsync(IEnumerable<Ascent> ascents);
    Task<int> SaveChangesAsync();
}
