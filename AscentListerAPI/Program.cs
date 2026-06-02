using AscentListerAPI;
using AscentListerAPI.Data;
using AscentListerAPI.Data.Repositories;
using AscentListerAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi("v1", options => { options.AddDocumentTransformer<BearerSecuritySchemeTransformer>(); });

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAscentListerService, AscentListerService>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IRouteRepository, RouteRepository>();
builder.Services.AddScoped<IAscentRepository, AscentRepository>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
        {
            var jwtSettings = builder.Configuration.GetSection("Jwt");
            
            options.Authority = jwtSettings["Authority"];
            options.RequireHttpsMetadata = jwtSettings.GetValue<bool>("RequireHttpsMetadata");
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSettings["ValidIssuer"],
                ValidateAudience = true,
                ValidAudience = jwtSettings["ValidAudience"],
            };
        }
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Ascent Lister API")
            .WithTheme(ScalarTheme.Moon)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);

        options
            .AddPreferredSecuritySchemes("Bearer")
            .AddHttpAuthentication("Bearer", auth => { auth.Token = ""; })
            .EnablePersistentAuthentication();
    });

}
app.MapGet("/", () => "API is running");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program;