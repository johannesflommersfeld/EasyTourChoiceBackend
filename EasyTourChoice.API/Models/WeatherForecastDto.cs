namespace EasyTourChoice.API.Models;

public record WeatherForecastDto
{
    required public MetaDto Meta { get; set; }

    required public List<ForecastTimeStepDto> Timeseries { get; set; }
}

public record MetaDto
{
    required public DateTime UpdatedAt { get; set; }

    required public ForecastUnitsDto Units { get; set; }
}

// TODO: remove everything that is not used in the frontend
public record ForecastUnitsDto
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

public record ForecastTimeStepDto
{
    required public DateTime Time { get; set; }

    required public ForecastDataDto Data { get; set; }
}

public record ForecastDataDto
{
    // Parameters which applies to this exact point in time.
    required public ForecastTimeInstantDto Instant { get; set; }

    // Parameters with validity times over one hour. Will not exist for all time steps.
    required public ForecastTimePeriodDto NextOneHours { get; set; }
}

// TODO: remove everything that is not needed in the frontend
public record ForecastTimeInstantDto
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

public record ForecastTimePeriodDto
{
    required public ForecastTimePeriodDetailsDto? Details { get; set; }

    required public WeatherSymbol? SymbolCode { get; set; }
}

// TODO: remove everything that is not needed in the frontend
public record ForecastTimePeriodDetailsDto
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

public enum WeatherSymbol
{
    CLEARSKY_DAY,
    CLEARSKY_NIGHT,
    CLEARSKY_POLARTWILIGHT,
    FAIR_DAY,
    FAIR_NIGHT,
    FAIR_POLARTWILIGHT,
    LIGHTS_SNOWSHOWERS_AND_THUNDER_DAY,
    LIGHTS_SNOWSHOWERS_AND_THUNDER_NIGHT,
    LIGHTS_SNOWSHOWERS_AND_THUNDER_POLARTWILIGHT,
    LIGHTS_SNOWSHOWERS_DAY,
    LIGHTS_SNOWSHOWERS_NIGHT,
    LIGHTS_SNOWSHOWERS_POLARTWILIGHT,
    HEAVY_RAIN_AND_THUNDER,
    HEAVY_SNOW_AND_THUNDER,
    RAIN_AND_THUNDER,
    HEAVY_SLEET_SHOWERS_AND_THUNDER_DAY,
    HEAVY_SLEET_SHOWERS_AND_THUNDER_NIGHT,
    HEAVY_SLEET_SHOWERS_AND_THUNDER_POLARTWILIGHT,
    HEAVY_SNOW,
    HEAVY_RAINSHOWERS_DAY,
    HEAVY_RAINSHOWERS_NIGHT,
    HEAVY_RAINSHOWERS_POLARTWILIGHT,
    LIGHTSLEET,
    HEAVY_RAIN,
    LIGHT_RAINSHOWERS_DAY,
    LIGHT_RAINSHOWERS_NIGHT,
    LIGHT_RAINSHOWERS_POLARTWILIGHT,
    HEAVY_SLEET_SHOWERS_DAY,
    HEAVY_SLEET_SHOWERS_NIGHT,
    HEAVY_SLEET_SHOWERS_POLARTWILIGHT,
    LIGHTSLEET_SHOWERS_DAY,
    LIGHTSLEET_SHOWERS_NIGHT,
    LIGHTSLEET_SHOWERS_POLARTWILIGHT,
    SNOW,
    HEAVY_RAINSHOWERS_AND_THUNDER_DAY,
    HEAVY_RAINSHOWERS_AND_THUNDER_NIGHT,
    HEAVY_RAINSHOWERS_AND_THUNDER_POLARTWILIGHT,
    SNOWSHOWERS_DAY,
    SNOWSHOWERS_NIGHT,
    SNOWSHOWERS_POLARTWILIGHT,
    FOG,
    SNOWSHOWERS_AND_THUNDER_DAY,
    SNOWSHOWERS_AND_THUNDER_NIGHT,
    SNOWSHOWERS_AND_THUNDER_POLARTWILIGHT,
    LIGHT_SNOW_AND_THUNDER,
    HEAVY_SLEET_AND_THUNDER,
    LIGHT_RAIN,
    RAINSHOWERS_AND_THUNDER_DAY,
    RAINSHOWERS_AND_THUNDER_NIGHT,
    RAINSHOWERS_AND_THUNDER_POLARTWILIGHT,
    RAIN,
    LIGHT_SNOW,
    LIGHT_RAINSHOWERS_AND_THUNDER_DAY,
    LIGHT_RAINSHOWERS_AND_THUNDER_NIGHT,
    LIGHT_RAINSHOWERS_AND_THUNDER_POLARTWILIGHT,
    HEAVY_SLEET,
    SLEET_AND_THUNDER,
    LIGHT_RAIN_AND_THUNDER,
    SLEET,
    LIGHTS_SLEET_SHOWERS_AND_THUNDER_DAY,
    LIGHTS_SLEET_SHOWERS_AND_THUNDER_NIGHT,
    LIGHTS_SLEET_SHOWERS_AND_THUNDER_POLARTWILIGHT,
    LIGHTSLEET_AND_THUNDER,
    PARTLY_CLOUDY_DAY,
    PARTLY_CLOUDY_NIGHT,
    PARTLY_CLOUDY_POLARTWILIGHT,
    SLEET_SHOWERS_AND_THUNDER_DAY,
    SLEET_SHOWERS_AND_THUNDER_NIGHT,
    SLEET_SHOWERS_AND_THUNDER_POLARTWILIGHT,
    RAINSHOWERS_DAY,
    RAINSHOWERS_NIGHT,
    RAINSHOWERS_POLARTWILIGHT,
    SNOW_AND_THUNDER,
    LEET_SHOWERS_DAY,
    SLEET_SHOWERS_NIGHT,
    SLEET_SHOWERS_POLARTWILIGHT,
    CLOUDY,
    HEAVY_SNOWSHOWERS_AND_THUNDER_DAY,
    HEAVY_SNOWSHOWERS_AND_THUNDER_NIGHT,
    HEAVY_SNOWSHOWERS_AND_THUNDER_POLARTWILIGHT,
}
