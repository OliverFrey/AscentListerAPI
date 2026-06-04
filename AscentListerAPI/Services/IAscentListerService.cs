using AscentListerAPI.Models;

namespace AscentListerAPI.Services;

/// <summary>
/// Application logic for reading and recording ascents, sitting between the
/// controllers and the repositories.
/// </summary>
public interface IAscentListerService
{
    /// <summary>
    /// Returns every ascent with its route and location eagerly loaded.
    /// </summary>
    Task<List<Ascent>> GetAllAscentsAsync();

    /// <summary>
    /// Persists a batch of ascents, reusing existing locations and routes where
    /// they already exist and creating the missing ones.
    /// </summary>
    /// <param name="ascents">The ascents to record.</param>
    /// <returns>The persisted ascents, including newly created routes and locations.</returns>
    Task<List<Ascent>> AddAscentsAsync(List<Ascent> ascents);
}
