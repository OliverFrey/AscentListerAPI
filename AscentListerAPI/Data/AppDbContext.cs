using AscentListerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Route = AscentListerAPI.Models.Route;

namespace AscentListerAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Ascent> Ascents => Set<Ascent>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Route> Routes => Set<Route>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.LocationId);
            entity.Property(e => e.LocationName).IsRequired();
            entity.Property(e => e.LocationAreaName).IsRequired();
            entity.Property(e => e.locationCountry).IsRequired();

            entity.HasMany<Route>()
                .WithOne()
                .HasForeignKey(e => e.LocationId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Route>(entity =>
        {
            entity.HasKey(e => e.RouteId);
            entity.Property(e => e.RouteName).IsRequired();
            entity.Property(e => e.Grade).IsRequired();

            entity.HasMany<Ascent>()
                .WithOne()
                .HasForeignKey(e => e.RouteId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Ascent>(entity =>
        {
            entity.HasKey(e => e.AscentId);
            entity.Property(e => e.Style).IsRequired();
            entity.Property(e => e.Comments).IsRequired();
        });
    }
}