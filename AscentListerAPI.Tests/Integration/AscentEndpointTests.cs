using System.Net;
using System.Net.Http.Json;
using AscentListerAPI.Models;
using AscentListerAPI.Tests.Fixtures;
using Xunit;

namespace AscentListerAPI.Tests.Integration;

public class AscentEndpointTests
{
    [Fact]
    public async Task UnauthenticatedRequest_Returns401()
    {
        await using var factory = new AscentApiFactory();
        var client = factory.CreateClient();

        var response = await client.GetAsync("/api/ascent");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task PostBatchThenGet_RoundTripsThroughHttp()
    {
        await using var factory = new AscentApiFactory();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add(TestAuthHandler.HeaderName, "1");

        var payload = AscentFixtures.TwoAscentsSharingNoLocations();

        var postResponse = await client.PostAsJsonAsync("/api/ascent/batch", payload);
        Assert.Equal(HttpStatusCode.OK, postResponse.StatusCode);

        var getResponse = await client.GetAsync("/api/ascent");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var stored = await getResponse.Content.ReadFromJsonAsync<List<Ascent>>();
        Assert.NotNull(stored);
        Assert.Equal(2, stored.Count);
        Assert.All(stored, a => Assert.NotNull(a.Route));
        Assert.All(stored, a => Assert.NotNull(a.Route.Location));
    }
}
