using Newtonsoft.Json;
using AutoMapper;
using EasyTourChoice.API.Controllers.Interfaces;
using EasyTourChoice.API.Application.Models;
using EasyTourChoice.API.Domain;
using EasyTourChoice.API.Repositories.Interfaces;

namespace EasyTourChoice.API.Application.DataAggregation;

public class YRWeatherForecastService(
    ILogger<YRWeatherForecastService> logger,
    IHttpService httpService,
    IMapper mapper,
    IWeatherForecastRepository weatherForecastRepository
) : IWeatherForecastService
{
    private readonly ILogger _logger = logger;
    private readonly IHttpService _httpService = httpService;
    private readonly IMapper _mapper = mapper;
    private readonly IWeatherForecastRepository _weatherRepository = weatherForecastRepository;

    public async Task<WeatherForecastDto?> GetWeatherForecastAsync(Location location)
    {
        var forecast = _weatherRepository.GetReportByLocation(RoundLocation(location));
        if (forecast is null || !IsValid(forecast))
        {
            await FetchWeatherForecastAsync(RoundLocation(location));
            forecast = _weatherRepository.GetReportByLocation(RoundLocation(location));
        }

        return _mapper.Map<WeatherForecastDto>(forecast);
    }

    private async Task FetchWeatherForecastAsync(Location location)
    {
        var userAgents = "github.com/johannesflommersfeld/EasyTourChoice/dev jf-dev@gmx.de";
        await using Stream stream =
            await _httpService.PerformGetRequestAsync(GetURL(location), userAgents);
        using var reader = new JsonTextReader(new StreamReader(stream));
        try
        {
            var serializer = new JsonSerializer();
            var response = serializer.Deserialize<YRResponse>(reader);
            var forecast = _mapper.Map<WeatherForecast>(response);
            _weatherRepository.SaveForecast(RoundLocation(location), forecast);
        }
        catch (HttpRequestException e)
        {
            _logger.LogError("{Message}", e.Message);
        }
    }

    private static bool IsValid(WeatherForecast forecast)
    {
        TimeSpan age = DateTime.Now - forecast.Meta.UpdatedAt;
        return age.TotalMinutes < 60;
    }

    private string GetURL(Location location)
    {
        var locationRounded = RoundLocation(location);
        // TODO: move string to configuration file
        string baseURL = "https://api.met.no/weatherapi/locationforecast/2.0/";
        if (location.Altitude is null)
        {
            return baseURL + $"compact?lat={locationRounded.Latitude}&lon={locationRounded.Longitude}";
        }
        else
        {
            return baseURL + $"compact?altitude={locationRounded.Altitude}&lat={locationRounded.Latitude}&lon={locationRounded.Longitude}";
        }
    }

    private static Location RoundLocation(Location location)
    {
        // this should ensure a precision of about 10km.
        return new Location()
        {
            Longitude = Math.Round(location.Longitude, 1),
            Latitude = Math.Round(location.Latitude, 1),
            Altitude = location.Altitude is null ? null : Math.Round((double)location.Altitude, 1),
        };
    }
}

internal record YRResponse
{
    [JsonProperty("type", Required = Required.Always)]
    required public YRType Type { get; set; }

    [JsonProperty("geometry", Required = Required.Always)]
    required public YRPointGeometry Geometry { get; set; }

    [JsonProperty("properties", Required = Required.Always)]
    required public YRForecast Forecast { get; set; }
}

internal enum YRType
{
    [System.Runtime.Serialization.EnumMember(Value = "Feature")]
    FEATURE,
    [System.Runtime.Serialization.EnumMember(Value = "Point")]
    POINT,
}

internal record YRPointGeometry
{
    [JsonProperty("type", Required = Required.Always)]
    required public YRType Type { get; set; }

    [JsonProperty("coordinates", Required = Required.Always)]
    required public List<double> Coordinates { get; set; }
}

internal record YRForecast
{
    [JsonProperty("meta", Required = Required.Always)]
    required public YRMeta Meta { get; set; }

    [JsonProperty("timeseries", Required = Required.Always)]
    required public List<YRForecastTimeStep> Timeseries { get; set; }
}

internal record YRMeta
{
    [JsonProperty("updated_at", Required = Required.Always)]
    required public DateTime UpdatedAt { get; set; }

    [JsonProperty("units", Required = Required.Always)]
    required public YRForecastUnits Units { get; set; }
}

internal record YRForecastUnits
{
    [JsonProperty("air_pressure_at_sea_level", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public string? AirPressureAtSeaLevel { get; set; }

    [JsonProperty("air_temperature", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public string? AirTemperature { get; set; }

    [JsonProperty("air_temperature_max", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public string? AirTemperatureMax { get; set; }

    [JsonProperty("air_temperature_min", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public string? AirTemperatureMin { get; set; }

    [JsonProperty("cloud_area_fraction", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public string? CloudAreaFraction { get; set; }

    [JsonProperty("cloud_area_fraction_high", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public string? CloudAreaFractionHigh { get; set; }

    [JsonProperty("cloud_area_fraction_low", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public string? CloudAreaFractionLow { get; set; }

    [JsonProperty("cloud_area_fraction_medium", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public string? CloudAreaFractionMedium { get; set; }

    [JsonProperty("dew_point_temperature", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public string? DewPointTemperature { get; set; }

    [JsonProperty("fog_area_fraction", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public string? FogAreaFraction { get; set; }

    [JsonProperty("precipitation_amount", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public string? PrecipitationAmount { get; set; }

    [JsonProperty("precipitation_amount_max", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public string? PrecipitationAmountMax { get; set; }

    [JsonProperty("precipitation_amount_min", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public string? PrecipitationAmountMin { get; set; }

    [JsonProperty("probability_of_precipitation", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public string? ProbabilityOfPrecipitation { get; set; }

    [JsonProperty("probability_of_thunder", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public string? ProbabilityOfThunder { get; set; }

    [JsonProperty("relative_humidity", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public string? RelativeHumidity { get; set; }

    [JsonProperty("ultraviolet_index_clear_sky_max", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public string? UVIndexClearSkyMax { get; set; }

    [JsonProperty("wind_from_direction", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public string? WindFromDirection { get; set; }

    [JsonProperty("wind_speed", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public string? WindSpeed { get; set; }

    [JsonProperty("wind_speed_of_gust", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public string? WindSpeedOfGusts { get; set; }
}

internal record YRForecastTimeStep
{
    [JsonProperty("time", Required = Required.Always)]
    required public DateTime Time { get; set; }

    [JsonProperty("data", Required = Required.Always)]
    required public YRForecastData Data { get; set; }
}

internal record YRForecastData
{
    // Parameters which applies to this exact point in time.
    [JsonProperty("instant", Required = Required.Always)]
    required public YRForecastTimeInstant Instant { get; set; }

    // Parameters with validity times over one hour. Will not exist for all time steps.
    [JsonProperty("next_1_hours")]
    required public YRForecastTimePeriod NextOneHours { get; set; }

    // Parameters with validity times over six hours. Will not exist for all time steps.
    [JsonProperty("next_6_hours")]
    required public YRForecastTimePeriod NextSixHours { get; set; }

    // Parameters with validity times over twelve hours. Will not exist for all time steps
    [JsonProperty("next_12_hours")]
    required public YRForecastTimePeriod NextTwelveHours { get; set; }
}

internal record YRForecastTimeInstant
{

    [JsonProperty("details", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public YRForecastTimeInstantDetails? Details { get; set; }
}

internal record YRForecastTimeInstantDetails
{
    [JsonProperty("air_pressure_at_sea_level", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public double? AirPressureAtSeaLevel { get; set; }

    [JsonProperty("air_temperature", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public double? AirTemperature { get; set; }

    [JsonProperty("cloud_area_fraction", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public double? CloudAreaFraction { get; set; }

    [JsonProperty("cloud_area_fraction_high", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public double? CloudAreaFractionHigh { get; set; }

    [JsonProperty("cloud_area_fraction_low", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public double? CloudAreaFractionLow { get; set; }

    [JsonProperty("cloud_area_fraction_medium", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public double? CloudAreaFractionMedium { get; set; }

    [JsonProperty("dew_point_temperature", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public double? DewPointTemperature { get; set; }

    [JsonProperty("fog_area_fraction", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public double? FogAreaFraction { get; set; }

    [JsonProperty("relative_humidity", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public double? RelativeHumidity { get; set; }

    [JsonProperty("wind_from_direction", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public double? WindFromDirection { get; set; }

    [JsonProperty("wind_speed", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public double? WindSpeed { get; set; }

    [JsonProperty("wind_speed_of_gust", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public double? WindSpeedOfGusts { get; set; }
}

internal record YRForecastTimePeriod
{
    [JsonProperty("details", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public YRForecastTimePeriodDetails? Details { get; set; }

    [JsonProperty("summary", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public YRForecastSummary? Summary { get; set; }
}

internal record YRForecastTimePeriodDetails
{
    [JsonProperty("air_temperature_max", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public double? AirTemperatureMax { get; set; }

    [JsonProperty("air_temperature_min", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public double? AirTemperatureMin { get; set; }

    [JsonProperty("precipitation_amount", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public double? PrecipitationAmount { get; set; }

    [JsonProperty("precipitation_amount_max", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public double? PrecipitationAmountMax { get; set; }

    [JsonProperty("precipitation_amount_min", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public double? PrecipitationAmountMin { get; set; }

    [JsonProperty("probability_of_precipitation", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public double? ProbabilityOfPrecipitation { get; set; }

    [JsonProperty("probability_of_thunder", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public double? ProbabilityOfThunder { get; set; }

    [JsonProperty("ultraviolet_index_clear_sky_max", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public double? UVIndexClearSkyMax { get; set; }
}

internal record YRForecastSummary
{
    [JsonProperty("symbol_code", Required = Required.Always)]
    required public WeatherSymbol SymbolCode { get; set; }
}

public enum WeatherSymbol
{
    [System.Runtime.Serialization.EnumMember(Value = "clearsky_day")]
    CLEARSKY_DAY,
    [System.Runtime.Serialization.EnumMember(Value = "clearsky_night")]
    CLEARSKY_NIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "clearsky_polartwilight")]
    CLEARSKY_POLARTWILIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "fair_day")]
    FAIR_DAY,
    [System.Runtime.Serialization.EnumMember(Value = "fair_night")]
    FAIR_NIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "fair_polartwilight")]
    FAIR_POLARTWILIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "lightssnowshowersandthunder_day")]
    LIGHTS_SNOWSHOWERS_AND_THUNDER_DAY,
    [System.Runtime.Serialization.EnumMember(Value = "lightssnowshowersandthunder_night")]
    LIGHTS_SNOWSHOWERS_AND_THUNDER_NIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "lightssnowshowersandthunder_polartwilight")]
    LIGHTS_SNOWSHOWERS_AND_THUNDER_POLARTWILIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "lightsnowshowers_day")]
    LIGHTS_SNOWSHOWERS_DAY,
    [System.Runtime.Serialization.EnumMember(Value = "lightsnowshowers_night")]
    LIGHTS_SNOWSHOWERS_NIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "lightsnowshowers_polartwilight")]
    LIGHTS_SNOWSHOWERS_POLARTWILIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "heavyrainandthunder")]
    HEAVY_RAIN_AND_THUNDER,
    [System.Runtime.Serialization.EnumMember(Value = "heavysnowandthunder")]
    HEAVY_SNOW_AND_THUNDER,
    [System.Runtime.Serialization.EnumMember(Value = "rainandthunder")]
    RAIN_AND_THUNDER,
    [System.Runtime.Serialization.EnumMember(Value = "heavysleetshowersandthunder_day")]
    HEAVY_SLEET_SHOWERS_AND_THUNDER_DAY,
    [System.Runtime.Serialization.EnumMember(Value = "heavysleetshowersandthunder_night")]
    HEAVY_SLEET_SHOWERS_AND_THUNDER_NIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "heavysleetshowersandthunder_polartwilight")]
    HEAVY_SLEET_SHOWERS_AND_THUNDER_POLARTWILIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "heavysnow")]
    HEAVY_SNOW,
    [System.Runtime.Serialization.EnumMember(Value = "heavyrainshowers_day")]
    HEAVY_RAINSHOWERS_DAY,
    [System.Runtime.Serialization.EnumMember(Value = "heavyrainshowers_night")]
    HEAVY_RAINSHOWERS_NIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "heavyrainshowers_polartwilight")]
    HEAVY_RAINSHOWERS_POLARTWILIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "lightsleet")]
    LIGHTSLEET,
    [System.Runtime.Serialization.EnumMember(Value = "heavyrain")]
    HEAVY_RAIN,
    [System.Runtime.Serialization.EnumMember(Value = "lightrainshowers_day")]
    LIGHT_RAINSHOWERS_DAY,
    [System.Runtime.Serialization.EnumMember(Value = "lightrainshowers_night")]
    LIGHT_RAINSHOWERS_NIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "lightrainshowers_polartwilight")]
    LIGHT_RAINSHOWERS_POLARTWILIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "heavysleetshowers_day")]
    HEAVY_SLEET_SHOWERS_DAY,
    [System.Runtime.Serialization.EnumMember(Value = "heavysleetshowers_night")]
    HEAVY_SLEET_SHOWERS_NIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "heavysleetshowers_polartwilight")]
    HEAVY_SLEET_SHOWERS_POLARTWILIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "lightsleetshowers_day")]
    LIGHTSLEET_SHOWERS_DAY,
    [System.Runtime.Serialization.EnumMember(Value = "lightsleetshowers_night")]
    LIGHTSLEET_SHOWERS_NIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "lightsleetshowers_polartwilight")]
    LIGHTSLEET_SHOWERS_POLARTWILIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "snow")]
    SNOW,
    [System.Runtime.Serialization.EnumMember(Value = "heavyrainshowersandthunder_day")]
    HEAVY_RAINSHOWERS_AND_THUNDER_DAY,
    [System.Runtime.Serialization.EnumMember(Value = "heavyrainshowersandthunder_night")]
    HEAVY_RAINSHOWERS_AND_THUNDER_NIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "heavyrainshowersandthunder_polartwilight")]
    HEAVY_RAINSHOWERS_AND_THUNDER_POLARTWILIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "snowshowers_day")]
    SNOWSHOWERS_DAY,
    [System.Runtime.Serialization.EnumMember(Value = "snowshowers_night")]
    SNOWSHOWERS_NIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "snowshowers_polartwilight")]
    SNOWSHOWERS_POLARTWILIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "fog")]
    FOG,
    [System.Runtime.Serialization.EnumMember(Value = "snowshowersandthunder_day")]
    SNOWSHOWERS_AND_THUNDER_DAY,
    [System.Runtime.Serialization.EnumMember(Value = "snowshowersandthunder_night")]
    SNOWSHOWERS_AND_THUNDER_NIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "snowshowersandthunder_polartwilight")]
    SNOWSHOWERS_AND_THUNDER_POLARTWILIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "lightsnowandthunder")]
    LIGHT_SNOW_AND_THUNDER,
    [System.Runtime.Serialization.EnumMember(Value = "heavysleetandthunder")]
    HEAVY_SLEET_AND_THUNDER,
    [System.Runtime.Serialization.EnumMember(Value = "lightrain")]
    LIGHT_RAIN,
    [System.Runtime.Serialization.EnumMember(Value = "rainshowersandthunder_day")]
    RAINSHOWERS_AND_THUNDER_DAY,
    [System.Runtime.Serialization.EnumMember(Value = "rainshowersandthunder_night")]
    RAINSHOWERS_AND_THUNDER_NIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "rainshowersandthunder_polartwilight")]
    RAINSHOWERS_AND_THUNDER_POLARTWILIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "rain")]
    RAIN,
    [System.Runtime.Serialization.EnumMember(Value = "lightsnow")]
    LIGHT_SNOW,
    [System.Runtime.Serialization.EnumMember(Value = "lightrainshowersandthunder_day")]
    LIGHT_RAINSHOWERS_AND_THUNDER_DAY,
    [System.Runtime.Serialization.EnumMember(Value = "lightrainshowersandthunder_night")]
    LIGHT_RAINSHOWERS_AND_THUNDER_NIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "lightrainshowersandthunder_polartwilight")]
    LIGHT_RAINSHOWERS_AND_THUNDER_POLARTWILIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "heavysleet")]
    HEAVY_SLEET,
    [System.Runtime.Serialization.EnumMember(Value = "sleetandthunder")]
    SLEET_AND_THUNDER,
    [System.Runtime.Serialization.EnumMember(Value = "lightrainandthunder")]
    LIGHT_RAIN_AND_THUNDER,
    [System.Runtime.Serialization.EnumMember(Value = "sleet")]
    SLEET,
    [System.Runtime.Serialization.EnumMember(Value = "lightssleetshowersandthunder_day")]
    LIGHTS_SLEET_SHOWERS_AND_THUNDER_DAY,
    [System.Runtime.Serialization.EnumMember(Value = "lightssleetshowersandthunder_night")]
    LIGHTS_SLEET_SHOWERS_AND_THUNDER_NIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "lightssleetshowersandthunder_polartwilight")]
    LIGHTS_SLEET_SHOWERS_AND_THUNDER_POLARTWILIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "lightsleetandthunder")]
    LIGHTSLEET_AND_THUNDER,
    [System.Runtime.Serialization.EnumMember(Value = "partlycloudy_day")]
    PARTLY_CLOUDY_DAY,
    [System.Runtime.Serialization.EnumMember(Value = "partlycloudy_night")]
    PARTLY_CLOUDY_NIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "partlycloudy_polartwilight")]
    PARTLY_CLOUDY_POLARTWILIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "sleetshowersandthunder_day")]
    SLEET_SHOWERS_AND_THUNDER_DAY,
    [System.Runtime.Serialization.EnumMember(Value = "sleetshowersandthunder_night")]
    SLEET_SHOWERS_AND_THUNDER_NIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "sleetshowersandthunder_polartwilight")]
    SLEET_SHOWERS_AND_THUNDER_POLARTWILIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "rainshowers_day")]
    RAINSHOWERS_DAY,
    [System.Runtime.Serialization.EnumMember(Value = "rainshowers_night")]
    RAINSHOWERS_NIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "rainshowers_polartwilight")]
    RAINSHOWERS_POLARTWILIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "snowandthunder")]
    SNOW_AND_THUNDER,
    [System.Runtime.Serialization.EnumMember(Value = "leetshowers_day")]
    LEET_SHOWERS_DAY,
    [System.Runtime.Serialization.EnumMember(Value = "sleetshowers_night")]
    SLEET_SHOWERS_NIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "sleetshowers_polartwilight")]
    SLEET_SHOWERS_POLARTWILIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "cloudy")]
    CLOUDY,
    [System.Runtime.Serialization.EnumMember(Value = "heavysnowshowersandthunder_day")]
    HEAVY_SNOWSHOWERS_AND_THUNDER_DAY,
    [System.Runtime.Serialization.EnumMember(Value = "heavysnowshowersandthunder_night")]
    HEAVY_SNOWSHOWERS_AND_THUNDER_NIGHT,
    [System.Runtime.Serialization.EnumMember(Value = "heavysnowshowersandthunder_polartwilight")]
    HEAVY_SNOWSHOWERS_AND_THUNDER_POLARTWILIGHT,
}
