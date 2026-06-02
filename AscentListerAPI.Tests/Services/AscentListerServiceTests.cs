using AscentListerAPI.Data;
using AscentListerAPI.Data.Repositories;
using AscentListerAPI.Models;
using AscentListerAPI.Services;
using AscentListerAPI.Tests.Fixtures;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;
using Route = AscentListerAPI.Models.Route;

namespace AscentListerAPI.Tests.Services;

public class AscentListerServiceTests
{
    private readonly ILocationRepository _locations = Substitute.For<ILocationRepository>();
    private readonly IRouteRepository _routes = Substitute.For<IRouteRepository>();
    private readonly IAscentRepository _ascents = Substitute.For<IAscentRepository>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly ILogger<AscentListerService> _logger = Substitute.For<ILogger<AscentListerService>>();

    private AscentListerService Subject() =>
        new(_locations, _routes, _ascents, _uow, _logger);

    [Fact]
    public async Task AddAscentsAsync_AllNewGraph_AddsLocationRouteAndAscent()
    {
        _locations.GetByIdAsync(Arg.Any<int>()).Returns((Location?)null);
        _routes.GetByIdAsync(Arg.Any<int>()).Returns((Route?)null);

        var payload = AscentFixtures.TwoAscentsSharingNoLocations();
        var result = await Subject().AddAscentsAsync(payload);

        Assert.Equal(2, result.Count);
        await _locations.Received(2).AddAsync(Arg.Any<Location>());
        await _routes.Received(2).AddAsync(Arg.Any<Route>());
        await _ascents.Received(1).AddRangeAsync(payload);
        await _uow.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task AddAscentsAsync_LocationExists_ReusesAndSkipsAdd()
    {
        var existingCrag = AscentFixtures.TestCrag();
        _locations.GetByIdAsync(existingCrag.LocationId).Returns(existingCrag);
        _routes.GetByIdAsync(Arg.Any<int>()).Returns((Route?)null);

        var payload = new List<Ascent> { AscentFixtures.FlashOnRouteOne() };
        await Subject().AddAscentsAsync(payload);

        await _locations.DidNotReceive().AddAsync(Arg.Any<Location>());
        await _routes.Received(1).AddAsync(Arg.Any<Route>());
        Assert.Same(existingCrag, payload[0].Route.Location);
    }

    [Fact]
    public async Task AddAscentsAsync_RouteExists_ReusesAndSkipsAdd()
    {
        var existingRoute = AscentFixtures.TestRouteOne();
        _locations.GetByIdAsync(Arg.Any<int>()).Returns((Location?)null);
        _routes.GetByIdAsync(existingRoute.RouteId).Returns(existingRoute);

        var payload = new List<Ascent> { AscentFixtures.FlashOnRouteOne() };
        await Subject().AddAscentsAsync(payload);

        await _routes.DidNotReceive().AddAsync(Arg.Any<Route>());
        Assert.Same(existingRoute, payload[0].Route);
    }

    [Fact]
    public async Task AddAscentsAsync_EmptyList_StillSaves()
    {
        var result = await Subject().AddAscentsAsync([]);

        Assert.Empty(result);
        await _locations.DidNotReceive().GetByIdAsync(Arg.Any<int>());
        await _routes.DidNotReceive().GetByIdAsync(Arg.Any<int>());
        await _ascents.Received(1).AddRangeAsync(Arg.Is<IEnumerable<Ascent>>(e => !e.Any()));
        await _uow.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task AddAscentsAsync_UnitOfWorkThrows_LogsAndRethrows()
    {
        _locations.GetByIdAsync(Arg.Any<int>()).Returns((Location?)null);
        _routes.GetByIdAsync(Arg.Any<int>()).Returns((Route?)null);
        _uow.SaveChangesAsync().Throws(new InvalidOperationException("boom"));

        var payload = AscentFixtures.TwoAscentsSharingNoLocations();

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => Subject().AddAscentsAsync(payload));
        Assert.Equal("boom", ex.Message);

        _logger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            Arg.Any<InvalidOperationException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public async Task GetAllAscentsAsync_DelegatesToRepository()
    {
        var expected = AscentFixtures.TwoAscentsSharingNoLocations();
        _ascents.GetAllWithGraphAsync().Returns(expected);

        var result = await Subject().GetAllAscentsAsync();

        Assert.Same(expected, result);
    }
}
