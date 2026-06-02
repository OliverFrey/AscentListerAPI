using AscentListerAPI.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AscentListerAPI.Tests.Integration;

public class AscentApiFactory : WebApplicationFactory<Program>
{
    public string DatabaseName { get; } = Guid.NewGuid().ToString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Authority"] = "https://example.test/",
                ["Jwt:RequireHttpsMetadata"] = "false",
                ["Jwt:ValidIssuer"] = "https://example.test/",
                ["Jwt:ValidAudience"] = "tests",
                ["ConnectionStrings:DefaultConnection"] = "Host=ignored",
            });
        });

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<AppDbContext>>();
            services.RemoveAll<AppDbContext>();

            var efServices = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            services.AddDbContext<AppDbContext>(opts => opts
                .UseInMemoryDatabase(DatabaseName)
                .UseInternalServiceProvider(efServices));

            services
                .AddAuthentication()
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    TestAuthHandler.SchemeName, _ => { });

            services.Configure<AuthenticationOptions>(opts =>
            {
                opts.DefaultAuthenticateScheme = TestAuthHandler.SchemeName;
                opts.DefaultChallengeScheme = TestAuthHandler.SchemeName;
                opts.DefaultScheme = TestAuthHandler.SchemeName;
            });
        });
    }
}
