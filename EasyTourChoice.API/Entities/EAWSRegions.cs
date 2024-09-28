
using Newtonsoft.Json;
namespace EasyTourChoice.API.Entities;

public record EAWSRegions
{
    [JsonProperty("type", Required = Required.Always)]
    required public RegionsType Type { get; set; }

    [JsonProperty("features", Required = Required.Always)]
    required public List<Feature> Features { get; set; }
}

public enum RegionsType
{
    [System.Runtime.Serialization.EnumMember(Value = @"FeatureCollection")]
    FeatureCollection = 0,
}

public record Feature
{
    [JsonProperty("type", Required = Required.Always)]
    required public FeatureType Type { get; set; }

    [JsonProperty("properties", Required = Required.Always)]
    required public Properties Properties { get; set; }

    [JsonProperty("geometry", Required = Required.Always)]
    required public Geometry Geometry { get; set; }
}

public enum FeatureType
{
    [System.Runtime.Serialization.EnumMember(Value = @"Feature")]
    Feature = 0,
}

public record Properties
{
    [JsonProperty("id", Required = Required.Always)]
    required public string Id { get; set; }
}

public record Geometry
{
    [JsonProperty("type", Required = Required.Always)]
    required public GeometryType Type { get; set; }

    [JsonProperty("coordinates", Required = Required.Always)]
    required public ICollection<ICollection<ICollection<ICollection<double>>>> Coordinates { get; set; }

    [JsonProperty("bbox", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<double>? Bbox { get; set; }
}

public enum GeometryType
{
    [System.Runtime.Serialization.EnumMember(Value = @"Point")]
    Point,
    [System.Runtime.Serialization.EnumMember(Value = @"LineString")]
    LineString,
    [System.Runtime.Serialization.EnumMember(Value = @"Polygon")]
    Polygon,
    [System.Runtime.Serialization.EnumMember(Value = @"MultiPoint")]
    MultiPoint,
    [System.Runtime.Serialization.EnumMember(Value = @"MultiLineString")]
    MultiLineString,
    [System.Runtime.Serialization.EnumMember(Value = @"MultiPolygon")]
    MultiPolygon,
}