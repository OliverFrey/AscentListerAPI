using AscentListerAPI.Data;
using AscentListerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Route = AscentListerAPI.Models.Route;

namespace AscentListerAPI.Services;

public class AscentListerService(AppDbContext context) : IAscentListerService
{
    public async Task<List<Ascent>> GetAllAscentsAsync()
        => await context.Ascents.ToListAsync();

    public async Task<Ascent?> GetAscentByIdAsync(int id)
    {
        var ascent = await context.Ascents.Where(a => a.AscentId == id).FirstOrDefaultAsync();
        return ascent;
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