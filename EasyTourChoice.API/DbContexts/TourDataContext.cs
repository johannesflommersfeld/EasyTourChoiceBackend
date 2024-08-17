using Microsoft.EntityFrameworkCore;
using EasyTourChoice.API.Entities;

namespace EasyTourChoice.API.DbContexts;

public class TourDataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<TourData> Tours { get; set; }
    public DbSet<Area> Areas { get; set; }
    public DbSet<Location> Locations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TourData>()
        .HasData(
            new TourData()
            {
                Id = 1,
                Name = "FantasySkitourSafiental",
                ActivityType = Activity.SKITOURING,
                StartingLocationId = 1,
                ActivityLocationId = 1,
                Duration = 5,
                MetersOfElevation = 1_200,
                ShortDescription = "Non existing ski tour for testing.",
                Risk = RiskLevel.MODERATE_RISK,
                AreaId = 1,
            },
            new TourData()
            {
                Id = 2,
                Name = "FantasySkitourStAntoenien",
                ActivityType = Activity.SKITOURING,
                StartingLocationId = 2,
                ActivityLocationId = 2,
                Duration = 5,
                MetersOfElevation = 1_200,
                ShortDescription = "Another non existing ski tour for testing.",
                Risk = RiskLevel.HIGH_RISK,
                AreaId = 2,
            }
        );

        modelBuilder.Entity<Area>()
        .HasData(
            new Area()
            {
                AreaId = 1,
                Name = "Safiental",
            },
            new Area()
            {
                AreaId = 2,
                Name = "St. Antoenien",
            }
        );

        modelBuilder.Entity<Location>()
        .HasData(
            new Location()
            {
                LocationId = 1,
                Latitude = 46.733224,
                Longitude = 9.335422,
                Altitude = 1_300,
            },
            new Location()
            {
                LocationId = 2,
                Latitude = 46.968062,
                Longitude = 9.815114,
                Altitude = 1_410,
            }
        );

        base.OnModelCreating(modelBuilder);
    }
}