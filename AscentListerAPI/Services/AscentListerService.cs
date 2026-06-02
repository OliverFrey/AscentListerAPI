using AscentListerAPI.Data;
using AscentListerAPI.Data.Repositories;
using AscentListerAPI.Models;
using Microsoft.Extensions.Logging;

namespace AscentListerAPI.Services;

public class AscentListerService(
    ILocationRepository locations,
    IRouteRepository routes,
    IAscentRepository ascents,
    IUnitOfWork unitOfWork,
    ILogger<AscentListerService> logger) : IAscentListerService
{
    public Task<List<Ascent>> GetAllAscentsAsync() => ascents.GetAllWithGraphAsync();

    public async Task<List<Ascent>> AddAscentsAsync(List<Ascent> incoming)
    {
        try
        {
            foreach (var ascent in incoming)
            {
                var location = await locations.GetByIdAsync(ascent.Route.Location.LocationId);
                if (location is null)
                {
                    await locations.AddAsync(ascent.Route.Location);
                }
                else
                {
                    ascent.Route.Location = location;
                }

                var route = await routes.GetByIdAsync(ascent.Route.RouteId);
                if (route is null)
                {
                    await routes.AddAsync(ascent.Route);
                }
                else
                {
                    ascent.Route = route;
                }
            }

            await ascents.AddRangeAsync(incoming);
            await unitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to add ascents batch of size {Count}", incoming.Count);
            throw;
        }

        return incoming;
    }
}
