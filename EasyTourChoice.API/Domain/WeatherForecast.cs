using EasyTourChoice.API.Application.DataAggregation;

namespace EasyTourChoice.API.Domain;

public record WeatherForecast
{
    required public Meta Meta { get; set; }

    required public List<ForecastTimeStep> Timeseries { get; set; }
}

public record Meta
{
    required public DateTime UpdatedAt { get; set; }

    required public ForecastUnits Units { get; set; }
}

// TODO: remove everything that is not used in the frontend
public record ForecastUnits
{
    required public string AirPressureAtSeaLevel { get; set; }
    required public string AirTemperature { get; set; }
    required public string AirTemperatureMax { get; set; }
    required public string AirTemperatureMin { get; set; }
    required public string CloudAreaFraction { get; set; }
    required public string CloudAreaFractionHigh { get; set; }
    required public string CloudAreaFractionLow { get; set; }
    required public string CloudAreaFractionMedium { get; set; }
    required public string DewPointTemperature { get; set; }
    required public string FogAreaFraction { get; set; }
    required public string PrecipitationAmount { get; set; }
    required public string PrecipitationAmountMax { get; set; }
    required public string PrecipitationAmountMin { get; set; }
    required public string ProbabilityOfPrecipitation { get; set; }
    required public string ProbabilityOfThunder { get; set; }
    required public string RelativeHumidity { get; set; }
    required public string UVIndexClearSkyMax { get; set; }
    required public string WindFromDirection { get; set; }
    required public string WindSpeed { get; set; }
    required public string WindSpeedOfGusts { get; set; }
}

public record ForecastTimeStep
{
    required public DateTime Time { get; set; }

    required public ForecastData Data { get; set; }
}

public record ForecastData
{
    // Parameters which applies to this exact point in time.
    required public ForecastTimeInstant Instant { get; set; }

    // Parameters with validity times over one hour. Will not exist for all time steps.
    required public ForecastTimePeriod NextOneHours { get; set; }
}

// TODO: remove everything that is not needed in the frontend
public record ForecastTimeInstant
{
    public double? AirPressureAtSeaLevel { get; set; }

    public double? AirTemperature { get; set; }

    public double? CloudAreaFraction { get; set; }

    public double? CloudAreaFractionHigh { get; set; }

    public double? CloudAreaFractionLow { get; set; }

    public double? CloudAreaFractionMedium { get; set; }

    public double? DewPointTemperature { get; set; }

    public double? FogAreaFraction { get; set; }

    public double? RelativeHumidity { get; set; }

    public double? WindFromDirection { get; set; }

    public double? WindSpeed { get; set; }

    public double? WindSpeedOfGusts { get; set; }
}

public record ForecastTimePeriod
{
    required public ForecastTimePeriodDetails? Details { get; set; }

    required public WeatherSymbol? SymbolCode { get; set; }
}

// TODO: remove everything that is not needed in the frontend
public record ForecastTimePeriodDetails
{
    public double? AirTemperatureMax { get; set; }

    public double? AirTemperatureMin { get; set; }

    public double? PrecipitationAmount { get; set; }

    public double? PrecipitationAmountMax { get; set; }

    public double? PrecipitationAmountMin { get; set; }

    public double? ProbabilityOfPrecipitation { get; set; }

    public double? ProbabilityOfThunder { get; set; }

    public double? UVIndexClearSkyMax { get; set; }
}
