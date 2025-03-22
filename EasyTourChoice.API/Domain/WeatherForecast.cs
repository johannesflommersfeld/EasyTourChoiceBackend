using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EasyTourChoice.API.Application.DataAggregation;

namespace EasyTourChoice.API.Domain;

public record WeatherForecast
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    required public Meta Meta { get; set; }

    required public List<ForecastTimeStep> Timeseries { get; set; }

    // Foreign key for Location
    public int? LocationId { get; set; }

    [ForeignKey("LocationId")]
    public Location? Location { get; set; }
}

public record Meta
{
    public required DateTime UpdatedAt { get; init; }

    public required ForecastUnits Units { get; init; }
}

// TODO: remove everything that is not used in the frontend
public record ForecastUnits
{
    public required string AirPressureAtSeaLevel { get; init; }
    public required string AirTemperature { get; init; }
    public required string AirTemperatureMax { get; init; }
    public required string AirTemperatureMin { get; init; }
    public required string CloudAreaFraction { get; init; }
    public required string CloudAreaFractionHigh { get; init; }
    public required string CloudAreaFractionLow { get; init; }
    public required string CloudAreaFractionMedium { get; init; }
    public required string DewPointTemperature { get; init; }
    public required string FogAreaFraction { get; init; }
    public required string PrecipitationAmount { get; init; }
    public required string PrecipitationAmountMax { get; init; }
    public required string PrecipitationAmountMin { get; init; }
    public required string ProbabilityOfPrecipitation { get; init; }
    public required string ProbabilityOfThunder { get; init; }
    public required string RelativeHumidity { get; init; }
    public required string UvIndexClearSkyMax { get; init; }
    public required string WindFromDirection { get; init; }
    public required string WindSpeed { get; init; }
    public required string WindSpeedOfGusts { get; init; }
}

public record ForecastTimeStep
{
    public required DateTime Time { get; init; }

    public required ForecastData Data { get; init; }
}

public record ForecastData
{
    // Parameters which applies to this exact point in time.
    public required ForecastTimeInstant Instant { get; init; }

    // Parameters with validity times over one hour. Will not exist for all time steps.
    public required ForecastTimePeriod NextOneHours { get; init; }
}

// TODO: remove everything that is not needed in the frontend
public record ForecastTimeInstant
{
    public double? AirPressureAtSeaLevel { get; init; }

    public double? AirTemperature { get; init; }

    public double? CloudAreaFraction { get; init; }

    public double? CloudAreaFractionHigh { get; init; }

    public double? CloudAreaFractionLow { get; init; }

    public double? CloudAreaFractionMedium { get; init; }

    public double? DewPointTemperature { get; init; }

    public double? FogAreaFraction { get; init; }

    public double? RelativeHumidity { get; init; }

    public double? WindFromDirection { get; init; }

    public double? WindSpeed { get; init; }

    public double? WindSpeedOfGusts { get; init; }
}

public record ForecastTimePeriod
{
    public required ForecastTimePeriodDetails? Details { get; init; }

    public required WeatherSymbol? SymbolCode { get; init; }
}

// TODO: remove everything that is not needed in the frontend
public record ForecastTimePeriodDetails
{
    public double? AirTemperatureMax { get; init; }

    public double? AirTemperatureMin { get; init; }

    public double? PrecipitationAmount { get; init; }

    public double? PrecipitationAmountMax { get; init; }

    public double? PrecipitationAmountMin { get; init; }

    public double? ProbabilityOfPrecipitation { get; init; }

    public double? ProbabilityOfThunder { get; init; }

    public double? UvIndexClearSkyMax { get; init; }
}
