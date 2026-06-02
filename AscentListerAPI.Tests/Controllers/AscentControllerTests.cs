using AscentListerAPI.Controllers;
using AscentListerAPI.Models;
using AscentListerAPI.Services;
using AscentListerAPI.Tests.Fixtures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace AscentListerAPI.Tests.Controllers;

public class AscentControllerTests
{
    private readonly IAscentListerService _service = Substitute.For<IAscentListerService>();

    private AscentController Subject() => new(_service);

    [Fact]
    public async Task GetAscents_ReturnsOkWithServicePayload()
    {
        var stored = AscentFixtures.TwoAscentsSharingNoLocations();
        _service.GetAllAscentsAsync().Returns(stored);

        var result = await Subject().GetAscents();

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(stored, ok.Value);
    }

    [Fact]
    public async Task AddAscents_ReturnsOkWithServicePayload()
    {
        var payload = AscentFixtures.TwoAscentsSharingNoLocations();
        _service.AddAscentsAsync(payload).Returns(payload);

        var result = await Subject().AddAscents(payload);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(payload, ok.Value);
    }

    [Fact]
    public void Controller_RequiresAuthorization()
    {
        var attr = typeof(AscentController)
            .GetCustomAttributes(typeof(AuthorizeAttribute), inherit: true);
        Assert.NotEmpty(attr);
    }
}
