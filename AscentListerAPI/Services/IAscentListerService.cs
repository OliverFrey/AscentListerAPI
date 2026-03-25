using AscentListerAPI.Models;

namespace AscentListerAPI.Services;

public interface IAscentListerService
{
    Task<List<Ascent>> GetAllAscentsAsync();
    Task<Ascent?> GetAscentByIdAsync(int id);
    Task<Ascent> AddAscentAsync(Ascent ascent);
    Task<Ascent?> UpdateAscentAsync(int id, Ascent ascent);
    Task<bool> DeleteAscentAsync(int id);
}