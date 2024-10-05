
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SQLitePCL;
namespace EasyTourChoice.API.Application.Models;

public record EAWSRegionsDto
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
    [JsonConverter(typeof(GeometryConverter))]
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



internal class GeometryConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Geometry);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        JObject jsonObject = JObject.Load(reader);
        var typeObject = jsonObject["type"]
            ?? throw new NullReferenceException("Geometry object does not contain a property 'type'");

        var geometry = new Geometry()
        {
            Type = (string)typeObject! switch
            {
                "Polygon" => GeometryType.Polygon,
                "MultiPolygon" => GeometryType.MultiPolygon,
                _ => throw new NotImplementedException(),
            },
            Coordinates = [],
        };

        var coordinatesObject = jsonObject["coordinates"]
            ?? throw new NullReferenceException("Json object does not contain a property 'coordinates'");
        var jsonArray = (JArray)coordinatesObject;
        if (geometry.Type == GeometryType.Polygon)
        {
            var convertedPolygon = jsonArray.ToObject<ICollection<ICollection<ICollection<double>>>>()
                ?? throw new NullReferenceException("Could not convert Polygon.");
            geometry.Coordinates.Add(convertedPolygon);
        }
        else
        {
            var convertedMultiPolygon = jsonArray.ToObject<ICollection<ICollection<ICollection<ICollection<double>>>>>()
                ?? throw new NullReferenceException("Could not convert MultiPolygon.");
            geometry.Coordinates = convertedMultiPolygon;
        }
        return geometry;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}