using AscentListerAPI.Data;
using AscentListerAPI.Data.Repositories;
using AscentListerAPI.Services;
using AscentListerAPI.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace AscentListerAPI.Tests.Integration;

public class AscentListerIntegrationTests
{
    private static (AscentListerService service, AppDbContext context) BuildSubject()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);
        var service = new AscentListerService(
            new LocationRepository(context),
            new RouteRepository(context),
            new AscentRepository(context),
            new UnitOfWork(context),
            NullLogger<AscentListerService>.Instance);

        return (service, context);
    }

    [Fact]
    public async Task AddAscentsAsync_PersistsFullGraphAndReturnsIncludingNestedEntities()
    {
        var (service, context) = BuildSubject();
        await using var _ = context;

        var payload = AscentFixtures.TwoAscentsSharingNoLocations();
        await service.AddAscentsAsync(payload);

        var stored = await service.GetAllAscentsAsync();

        Assert.Equal(2, stored.Count);
        Assert.All(stored, a => Assert.NotNull(a.Route));
        Assert.All(stored, a => Assert.NotNull(a.Route.Location));
        Assert.Contains(stored, a => a.Route.Location.LocationName == "Test Crag");
        Assert.Contains(stored, a => a.Route.Location.LocationName == "Another Test Crag");
    }

    [Fact]
    public async Task AddAscentsAsync_SharedLocation_OnlyOneLocationRow()
    {
        var (service, context) = BuildSubject();
        await using var _ = context;

        var payload = AscentFixtures.TwoAscentsSharingOneLocation();
        await service.AddAscentsAsync(payload);

        var locationRows = await context.Locations.AsNoTracking().ToListAsync();
        Assert.Single(locationRows);
        Assert.Equal("Test Crag", locationRows[0].LocationName);
    }

    [Fact]
    public async Task AddAscentsAsync_DuplicateAscentIdInBatch_Throws()
    {
        var (service, context) = BuildSubject();
        await using var _ = context;

        var payload = AscentFixtures.TwoAscentsSharingNoLocations();
        payload[1].AscentId = payload[0].AscentId;

        await Assert.ThrowsAnyAsync<Exception>(() => service.AddAscentsAsync(payload));
    }
}
