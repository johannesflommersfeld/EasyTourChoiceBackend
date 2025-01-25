using EasyTourChoice.API.Domain;
using EasyTourChoice.API.ValidationAttributes;
using Newtonsoft.Json;

namespace EasyTourChoice.API.Application.Models;

public record EAWSReportDto
{
    [JsonProperty("bulletins", Required = Required.Always)]
    required public List<EAWSBulletin> Bulletins { get; set; }
}

// Avalanche Bulletin valid for a given set of regions.
public record EAWSBulletin
{
    [JsonProperty("bulletinID", Required = Required.Always)]
    required public string BulletinID { get; set; }

    [JsonProperty("publicationTime", Required = Required.Always)]
    required public DateTime PublicationTime { get; set; }

    [JsonProperty("validTime", Required = Required.Always)]
    required public EAWSValidTime ValidTime { get; set; }

    [JsonProperty("avalancheProblems", Required = Required.Always)]
    required public List<EAWSAvalancheProblem> AvalancheProblems { get; set; }

    [JsonProperty("dangerRatings", Required = Required.Always)]
    required public List<EAWSDangerRating> DangerRatings { get; set; }

    [JsonProperty("avalancheActivity", Required = Required.Always)]
    required public EAWSAvalancheActivity AvalancheActivity { get; set; }

    [JsonProperty("snowpackStructure", Required = Required.Always)]
    required public EAWSSnowpackStructure SnowpackStructure { get; set; }

    [JsonProperty("tendency", Required = Required.Always)]
    required public List<EAWSTendency> Tendency { get; set; }

    [JsonProperty("weatherForecast", Required = Required.Always)]
    required public EAWSWeatherForecast WeatherForecast { get; set; }

    [JsonProperty("regions", Required = Required.Always)]
    required public List<EAWSRegion> Regions { get; set; }

    [JsonProperty("travelAdvisory", Required = Required.Always)]
    required public EAWSTravelAdvisory TravelAdvisory { get; set; }

    [JsonProperty("lang", Required = Required.Always)]
    required public string Language { get; set; }

    [JsonProperty("unscheduled", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
    required public bool Unscheduled { get; set; } = false;

    public bool IsValid()
    {
        return (ValidTime.StartTime > DateTime.Now) && (ValidTime.EndTime < DateTime.Now);
    }
}

// Defines two ISO 8601 timestamps in UTC or with time zone information.
public record EAWSValidTime
{
    [JsonProperty("startTime", Required = Required.Always)]
    required public DateTime StartTime { get; set; }

    [JsonProperty("endTime", Required = Required.Always)]
    required public DateTime EndTime { get; set; }
}

// Texts element with highlight and comment for the avalanche activity.
public record EAWSAvalancheActivity
{
    [JsonProperty("highlights", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
    required public string Highlights { get; set; }

    [JsonProperty("comment", Required = Required.Always)]
    required public string Comment { get; set; }
}

// Text element with highlight and comment for details on the snowpack structure.
public record EAWSSnowpackStructure
{
    [JsonProperty("highlights", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
    required public string Highlights { get; set; } = string.Empty;

    [JsonProperty("comment", Required = Required.Always)]
    required public string Comment { get; set; }
}

// A detailed description of the expected avalanche situation tendency after the bulletin's period of validity or
// for a given time period.
public record EAWSTendency
{
    [JsonProperty("highlights", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
    required public string Highlights { get; set; } = string.Empty;

    [JsonProperty("comment", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
    required public string Comment { get; set; } = string.Empty;

    [JsonProperty("tendencyType", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
    required public TendencyType TendencyType { get; set; } = TendencyType.UNKNOWN;

    [JsonProperty("validTime", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
    required public EAWSValidTime? ValidTime { get; set; } = null;
}

public enum TendencyType
{
    UNKNOWN,
    [System.Runtime.Serialization.EnumMember(Value = @"decreasing")]
    DECREASING,
    [System.Runtime.Serialization.EnumMember(Value = @"steady")]
    STEADY,
    [System.Runtime.Serialization.EnumMember(Value = @"increasing")]
    INCREASING,
}

// Highlight and comment for travel advisory.
public record EAWSTravelAdvisory
{
    [JsonProperty("highlights", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
    required public string Highlights { get; set; } = string.Empty;

    [JsonProperty("comment", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
    required public string Comment { get; set; } = string.Empty;
}

// Highlight and comment for weather forecast information.
public record EAWSWeatherForecast
{
    [JsonProperty("highlights", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
    required public string Highlights { get; set; } = string.Empty;

    [JsonProperty("comment", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
    required public string Comment { get; set; } = string.Empty;
}

// Defines an avalanche problem, its time, aspect, and elevation constraints. A textual detail about the affected
// terrain can be given in the comment field. Also, details about the expected avalanche size, snowpack stability 
// and its frequency can be defined. The implied danger rating value is optional.
public record EAWSAvalancheProblem
{
    [JsonProperty("problemType", Required = Required.Always)]
    required public AvalancheProblemType ProblemType { get; set; }

    [JsonProperty("elevation", Required = Required.Always)]
    required public EAWSElevation Elevation { get; set; }

    [JsonProperty("validTimePeriod", Required = Required.Always)]
    required public ValidTimePeriod ValidTimePeriod { get; set; }

    [JsonProperty("snowpackStability", Required = Required.Always)]
    required public SnowpackStability SnowpackStability { get; set; }

    [JsonProperty("frequency", Required = Required.Always)]
    required public Frequency Frequency { get; set; }

    [JsonProperty("avalancheSize", Required = Required.Always)]
    [AvalancheSize]
    required public int AvalancheSize { get; set; }

    [JsonProperty("aspects", Required = Required.Always)]
    required public List<Aspect> Aspects { get; set; }
}

// Expected avalanche problem, according to the EAWS avalanche problem definition.
public enum AvalancheProblemType
{
    [System.Runtime.Serialization.EnumMember(Value = @"no_distinct_avalanche_problem")]
    NONE,
    [System.Runtime.Serialization.EnumMember(Value = @"new_snow")]
    NEW_SNOW,
    [System.Runtime.Serialization.EnumMember(Value = @"wind_slab")]
    WIND_SLAB,
    [System.Runtime.Serialization.EnumMember(Value = @"persistent_weak_layers")]
    PERSISTENT_WEAK_LAYERS,
    [System.Runtime.Serialization.EnumMember(Value = @"wet_snow")]
    WET_SNOW,
    [System.Runtime.Serialization.EnumMember(Value = @"gliding_snow")]
    GLIDING_SNOW,
    [System.Runtime.Serialization.EnumMember(Value = @"cornices")]
    CORNICES,
}

// Elevation describes either an elevation range below a certain bound (only upperBound is set to a value)
// or above a certain bound (only lowerBound is set to a value). If both values are set to a value,
// an elevation band is defined by this property. The value uses a numeric value, not more detailed
// than 100m resolution. Additionally to the numeric values also 'treeline' is allowed.
public record EAWSElevation
{
    [JsonProperty("upperBound", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
    public string? UpperBound { get; set; }

    [JsonProperty("lowerBound", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
    public string? LowerBound { get; set; }
}

// Valid time period can be used to limit the validity of an element to an earlier or later period.
// It can be used to distinguish danger ratings or avalanche problems.
public enum ValidTimePeriod
{
    [System.Runtime.Serialization.EnumMember(Value = @"all_day")]
    ALL_DAY,
    [System.Runtime.Serialization.EnumMember(Value = @"earlier")]
    EARLIER,
    [System.Runtime.Serialization.EnumMember(Value = @"later")]
    LATER,
}

// Snowpack stability, according to the EAWS definition. Four stage scale (very poor, poor, fair, good).
public enum SnowpackStability
{
    [System.Runtime.Serialization.EnumMember(Value = @"good")]
    GOOD,
    [System.Runtime.Serialization.EnumMember(Value = @"fair")]
    FAIR,
    [System.Runtime.Serialization.EnumMember(Value = @"poor")]
    POOR,
    [System.Runtime.Serialization.EnumMember(Value = @"very_poor")]
    VERY_POOR,
}

// Expected frequency of lowest snowpack stability, according to the EAWS definition.
// Three stage scale (few, some, many).
public enum Frequency
{
    [System.Runtime.Serialization.EnumMember(Value = @"none")]
    NONE,
    [System.Runtime.Serialization.EnumMember(Value = @"few")]
    FEW,
    [System.Runtime.Serialization.EnumMember(Value = @"some")]
    SOME,
    [System.Runtime.Serialization.EnumMember(Value = @"many")]
    MANY,
}

// Defines a danger rating, its elevation constraints and the valid time period. If validTimePeriod or elevation
// are constrained for a rating, it is expected to define a dangerRating for all the other cases.
public record EAWSDangerRating
{
    [JsonProperty("mainValue", Required = Required.Always)]
    required public AvalancheDangerRating MainValue { get; set; }

    [JsonProperty("elevation", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
    public EAWSElevation? Elevation { get; set; }

    [JsonProperty("validTimePeriod", Required = Required.Always)]
    required public ValidTimePeriod ValidTimePeriod { get; set; }
}

// Danger rating value, according to EAWS danger scale definition.
public enum AvalancheDangerRating
{
    [System.Runtime.Serialization.EnumMember(Value = @"no_rating")]
    NO_RATING,
    [System.Runtime.Serialization.EnumMember(Value = @"low")]
    LOW,
    [System.Runtime.Serialization.EnumMember(Value = @"moderate")]
    MODERATE,
    [System.Runtime.Serialization.EnumMember(Value = @"considerable")]
    CONSIDERABLE,
    [System.Runtime.Serialization.EnumMember(Value = @"high")]
    HIGH,
    [System.Runtime.Serialization.EnumMember(Value = @"very_high")]
    VERY_HIGH,
    [System.Runtime.Serialization.EnumMember(Value = @"no_snow")]
    NO_SNOW,
}

// Region element describes a (micro) region. The regionID follows the EAWS schema.
// Additionally, the region name can be added.
public record EAWSRegion
{
    [JsonProperty("name", Required = Required.Always)]
    required public string Name { get; set; }

    [JsonProperty("regionID", Required = Required.Always)]
    required public string RegionID { get; set; }
}

