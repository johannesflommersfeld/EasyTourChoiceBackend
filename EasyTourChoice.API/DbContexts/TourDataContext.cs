using EasyTourChoice.API.Application.Models;
using EasyTourChoice.API.Domain;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace EasyTourChoice.API.DbContexts;

public class TourDataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<TourData> Tours { get; set; }
    public DbSet<Area> Areas { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<AvalancheReport> AvalancheReports { get; set; }
    public DbSet<DangerRating> DangerRatings { get; set; }
    public DbSet<AvalancheProblem> AvalancheProblems { get; set; }
    
    // TODO add weather report and travel information

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // configure Locations
        modelBuilder.Entity<Location>()
            .Property(e => e.LocationId)
            .ValueGeneratedOnAdd();
        
        // configure Areas
        modelBuilder.Entity<Area>()
            .Property(e => e.AreaId)
            .ValueGeneratedOnAdd();
        
        // configure TourData
        modelBuilder.Entity<TourData>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<TourData>()
            .Property(e => e.ActivityType)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<Activity>(v)
            );
        modelBuilder.Entity<TourData>()
            .Property(e => e.Difficulty)
            .HasConversion(
                v => v.ToString(),
                v => v == null ? null : Enum.Parse<GeneralDifficulty>(v)
            );
        modelBuilder.Entity<TourData>()
            .Property(e => e.Risk)
            .HasConversion(
                v => v.ToString(),
                v => v == null ? null : Enum.Parse<RiskLevel>(v)
            );
        
        // configure AvalancheReport
        modelBuilder.Entity<AvalancheReport>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<AvalancheReport>()
            .HasMany(r => r.DangerRatings)
            .WithMany(dr => dr.AvalancheReports)
            .UsingEntity(j => j.ToTable("ReportRatings"));        
        modelBuilder.Entity<AvalancheReport>()
            .HasMany(r => r.AvalancheProblems)
            .WithMany(p => p.AvalancheReports)
            .UsingEntity(j => j.ToTable("ReportProblems"));
        var reportBodyConverter = new ValueConverter<Dictionary<string, List<string>>, string>(
            v => JsonSerializer.Serialize(v, new JsonSerializerOptions { WriteIndented = true }), // Serialize, return empty string if null
            v => JsonSerializer.Deserialize<Dictionary<string, List<string>>>(v, new JsonSerializerOptions { WriteIndented = true }) 
                  ?? new Dictionary<string, List<string>>() // Deserialize, return empty dictionary if null or invalid
        );
        modelBuilder.Entity<AvalancheReport>()
            .Property(e => e.ReportBody)
            .HasConversion(reportBodyConverter);

        // configure AvalancheProblem
        modelBuilder.Entity<AvalancheProblem>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<AvalancheProblem>()
            .Property(e => e.ProblemType)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<AvalancheProblemType>(v)
            );
        modelBuilder.Entity<AvalancheProblem>()
            .Property(e => e.ValidTimePeriod)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<ValidTimePeriod>(v)
            );
        modelBuilder.Entity<AvalancheProblem>()
            .Property(e => e.SnowpackStability)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<SnowpackStability>(v)
            );
        modelBuilder.Entity<AvalancheProblem>()
            .Property(e => e.Frequency)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<Frequency>(v)
            );
        
        // configure DangerRatings
        modelBuilder.Entity<DangerRating>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<DangerRating>()
            .Property(e => e.MainValue)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<AvalancheDangerRating>(v)
            );
        modelBuilder.Entity<DangerRating>()
            .Property(e => e.ValidTimePeriod)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<ValidTimePeriod>(v)
            );

        base.OnModelCreating(modelBuilder);
    }

    public async Task SeedAsync()
    {
        
    await Database.EnsureCreatedAsync();
    
        if (!AvalancheReports.Any())
        {
            List<DangerRating> dangerRatings =
            [
                new DangerRating
                {
                    MainValue = AvalancheDangerRating.LOW,
                    ValidTimePeriod = ValidTimePeriod.ALL_DAY,
                    UpperBound = "treeline",
                    AvalancheReports = [],
                },
                new DangerRating
                {
                    MainValue = AvalancheDangerRating.MODERATE,
                    ValidTimePeriod = ValidTimePeriod.ALL_DAY,
                    LowerBound = "treeline",
                    AvalancheReports = [],
                }
            ];

            List<AvalancheProblem> avalancheProblems = 
            [
                new AvalancheProblem
                {
                    ProblemType = AvalancheProblemType.WIND_SLAB,
                    ValidTimePeriod = ValidTimePeriod.ALL_DAY,
                    LowerBound = "treeline",
                    SnowpackStability = SnowpackStability.FAIR,
                    Frequency = Frequency.FEW,
                    AvalancheSize = 2,
                    Aspect = (Aspect)0b1000_0011,
                    AvalancheReports = [],
                },
                new AvalancheProblem
                {
                    ProblemType = AvalancheProblemType.WET_SNOW,
                    ValidTimePeriod = ValidTimePeriod.ALL_DAY,
                    UpperBound = "treeline",
                    SnowpackStability = SnowpackStability.GOOD,
                    Frequency = Frequency.FEW,
                    AvalancheSize = 1,
                    Aspect = (Aspect)0b0011_1000,
                    AvalancheReports = [],
                }
            ];

            var avalancheReport = new AvalancheReport
            {
                PublicationTime = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now + TimeSpan.FromDays(1),
                ReportBody = new Dictionary<string, List<string>>
                {
                    ["test1"] = [ "bla", "blub" ],
                    ["test2"] = [ "hallo", "hallo" ]
                },
                Tendency = TendencyType.STEADY,
                AvalancheProblems = avalancheProblems,
                DangerRatings = dangerRatings,
            };

            foreach (var dangerRating in dangerRatings)
            {
                dangerRating.AvalancheReports.Add(avalancheReport);
            }

            foreach (var problem in avalancheProblems)
            {
                problem.AvalancheReports.Add(avalancheReport);
            }
            
            AvalancheReports.Add(avalancheReport);
            DangerRatings.AddRange(dangerRatings);
            AvalancheProblems.AddRange(avalancheProblems);
        }

        if (!Locations.Any())
        {
            Locations.AddRange(
                new Location
                {
                    Latitude = 46.733224,
                    Longitude = 9.335422,
                    Altitude = 1_300,
                },
                new Location
                {
                    Latitude = 46.968062,
                    Longitude = 9.815114,
                    Altitude = 1_410,
                }
            );
        }
        await SaveChangesAsync();

        if (!Areas.Any())
        {
            Areas.AddRange(
                new Area()
                {
                    Name = "Safiental",
                    LocationId = Locations.ToList()[0].LocationId,
                },
                new Area()
                {
                    Name = "St. Antoenien",
                    LocationId = Locations.ToList()[1].LocationId,
                }
            );
            await SaveChangesAsync();
        }

        if (!Tours.Any())
        {
            Tours.AddRange(
                new TourData()
                {
                    Name = "FantasySkitourSafiental",
                    ActivityType = Activity.SKITOURING,
                    StartingLocationId = Locations.ToList()[0].LocationId,
                    ActivityLocationId = Locations.ToList()[0].LocationId,
                    Duration = 5,
                    MetersOfElevation = 1_200,
                    ShortDescription = "Non existing ski tour for testing.",
                    Risk = RiskLevel.MODERATE_RISK,
                    AreaId = Areas.ToList()[0].AreaId,
                },
                new TourData()
                {
                    Name = "FantasySkitourStAntoenien",
                    ActivityType = Activity.SKITOURING,
                    StartingLocationId = Locations.ToList()[1].LocationId,
                    ActivityLocationId = Locations.ToList()[1].LocationId,
                    Duration = 5,
                    MetersOfElevation = 1_200,
                    ShortDescription = "Another non existing ski tour for testing.",
                    Risk = RiskLevel.HIGH_RISK,
                    AreaId = Areas.ToList()[1].AreaId,
                }
            );
            await SaveChangesAsync();
        }
    }
    
    public async Task CleanupExpiredAvalancheReportsAsync()
    {
        // Delete all expired avalanche reports
        var expiredReports = AvalancheReports
            .Where(ar => ar.EndTime < DateTime.Now)
            .ToList();
        AvalancheReports.RemoveRange(expiredReports);
        await SaveChangesAsync();

        // Delete all avalanche problems and danger ratings that are no longer referenced
        var unreferencedProblems = AvalancheProblems
            .Where(p => !p.AvalancheReports.Any())
            .ToList();

        var unreferencedRatings = DangerRatings
            .Where(r => !r.AvalancheReports.Any())
            .ToList();

        AvalancheProblems.RemoveRange(unreferencedProblems);
        DangerRatings.RemoveRange(unreferencedRatings);

        await SaveChangesAsync();
    }
}