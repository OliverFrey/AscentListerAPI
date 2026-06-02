using AscentListerAPI.Models;

namespace AscentListerAPI.Services;

public interface IAscentListerService
{
    Task<List<Ascent>> GetAllAscentsAsync();
    Task<List<Ascent>> AddAscentsAsync(List<Ascent> ascents);
}