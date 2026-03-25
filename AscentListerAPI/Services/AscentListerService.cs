using AscentListerAPI.Data;
using AscentListerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Route = AscentListerAPI.Models.Route;

namespace AscentListerAPI.Services;

public class AscentListerService(AppDbContext context) : IAscentListerService
{
    public async Task<List<Ascent>> GetAllAscentsAsync()
        => await context.Ascents.AsNoTracking()
            .Include(r => r.Route)
                .ThenInclude(r => r.Location)
            .ToListAsync();

    public async Task<Ascent?> GetAscentByIdAsync(int id)
    {
        var ascent = await context.Ascents
                .AsNoTracking()
                .Include(a => a.Route)
                    .ThenInclude(r => r.Location)
                .Where(a => a.AscentId == id).FirstOrDefaultAsync();
        return ascent;
    }

    public async Task<Ascent> AddAscentAsync(Ascent ascent)
    {
        await context.Ascents.AddAsync(ascent);
        await context.SaveChangesAsync();
        return ascent;
    }

    public async Task<Ascent?> UpdateAscentAsync(int id, Ascent ascent)
    {
        if (id != ascent.AscentId)
            return null;
        
        var existingAscent = await context.Ascents.FindAsync(id);
        if (existingAscent == null)
            return null;
        
        context.Ascents.Update(ascent);
        await context.SaveChangesAsync();
        return ascent;
    }

    public async Task<bool> DeleteAscentAsync(int id)
    {
        var ascent = await context.Ascents.FindAsync(id);
        if (ascent == null)
            return false;
        
        context.Ascents.Remove(ascent);
        await context.SaveChangesAsync();
        return true;
    }
}