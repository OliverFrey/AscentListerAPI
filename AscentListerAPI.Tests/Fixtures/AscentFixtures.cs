using AscentListerAPI.Models;
using Route = AscentListerAPI.Models.Route;

namespace AscentListerAPI.Tests.Fixtures;

public static class AscentFixtures
{
    public static Location TestCrag(int id = 1) => new()
    {
        LocationId = id,
        LocationName = "Test Crag",
        LocationAreaName = "Test Area",
        locationCountry = "Test Country",
        LocationStatus = StatusEnum.NEW,
    };

    public static Location AnotherTestCrag(int id = 2) => new()
    {
        LocationId = id,
        LocationName = "Another Test Crag",
        LocationAreaName = "Another Test Area",
        locationCountry = "Another Test Country",
        LocationStatus = StatusEnum.NEW,
    };

    public static Route TestRouteOne(Location? at = null) => new()
    {
        RouteId = 1,
        RouteName = "Test Route One",
        Grade = "6a",
        RouteStatus = StatusEnum.NEW,
        Location = at ?? TestCrag(),
    };

    public static Route TestRouteTwo(Location? at = null) => new()
    {
        RouteId = 2,
        RouteName = "Test Route Two",
        Grade = "6b",
        RouteStatus = StatusEnum.NEW,
        Location = at ?? AnotherTestCrag(),
    };

    public static Ascent FlashOnRouteOne() => new()
    {
        AscentId = 1,
        Date = new DateOnly(2026, 1, 1),
        Style = "Flash",
        Attempts = 1,
        Comments = "First ascent from fixture",
        Status = StatusEnum.NEW,
        Route = TestRouteOne(),
    };

    public static Ascent RedpointOnRouteTwo() => new()
    {
        AscentId = 2,
        Date = new DateOnly(2026, 1, 2),
        Style = "Redpoint",
        Attempts = 3,
        Comments = "Second ascent from fixture",
        Status = StatusEnum.NEW,
        Route = TestRouteTwo(),
    };

    public static List<Ascent> TwoAscentsSharingNoLocations() =>
        [FlashOnRouteOne(), RedpointOnRouteTwo()];

    public static List<Ascent> TwoAscentsSharingOneLocation()
    {
        var crag = TestCrag();
        return
        [
            new Ascent
            {
                AscentId = 10,
                Date = new DateOnly(2026, 2, 1),
                Style = "Onsight",
                Attempts = 1,
                Comments = "shared crag, route A",
                Status = StatusEnum.NEW,
                Route = new Route
                {
                    RouteId = 10,
                    RouteName = "Shared-Crag Route A",
                    Grade = "5c",
                    RouteStatus = StatusEnum.NEW,
                    Location = crag,
                },
            },
            new Ascent
            {
                AscentId = 11,
                Date = new DateOnly(2026, 2, 2),
                Style = "Redpoint",
                Attempts = 2,
                Comments = "shared crag, route B",
                Status = StatusEnum.NEW,
                Route = new Route
                {
                    RouteId = 11,
                    RouteName = "Shared-Crag Route B",
                    Grade = "6a+",
                    RouteStatus = StatusEnum.NEW,
                    Location = crag,
                },
            },
        ];
    }
}
