using System.Text.Json;
using AscentListerAPI.Data;
using AscentListerAPI.Models;
using AscentListerAPI.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AscentListerAPI.Tests.Services;

public class AscentListerServiceTests
{
    [Fact]
    public async Task AddAscentsAsync_AddsAllAscents_FromClientJsonPayload()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new AppDbContext(options);
        var service = new AscentListerService(context);

        var jsonPath = Path.Combine(
            AppContext.BaseDirectory,
            "TestData",
            "add-ascents-request.json");

        var json = await File.ReadAllTextAsync(jsonPath);

        var ascents = JsonSerializer.Deserialize<List<Ascent>>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        Assert.NotNull(ascents);

        var result = await service.AddAscentsAsync(ascents);
        var savedAscents = await service.GetAllAscentsAsync();

        Assert.Equal(2, result.Count);
        Assert.Equal(2, savedAscents.Count);

        Assert.Contains(savedAscents, ascent =>
            ascent.AscentId == 1 &&
            ascent.Style == "Flash" &&
            ascent.Route.RouteName == "Test Route One" &&
            ascent.Route.Location.LocationName == "Test Crag");

        Assert.Contains(savedAscents, ascent =>
            ascent.AscentId == 2 &&
            ascent.Style == "Redpoint" &&
            ascent.Route.RouteName == "Test Route Two" &&
            ascent.Route.Location.LocationName == "Another Test Crag");

        Assert.All(savedAscents, ascent =>
        {
            Assert.NotNull(ascent.Route);
            Assert.NotNull(ascent.Route.Location);
        });
    }
}
