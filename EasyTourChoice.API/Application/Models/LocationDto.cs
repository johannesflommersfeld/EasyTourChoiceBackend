using EasyTourChoice.API.Application.Models.BaseModels;
using System.Text.Json.Serialization;

namespace EasyTourChoice.API.Application.Models;

public class LocationDto : LocationBase
{
    [property: JsonPropertyName("locationId")]
    public int? LocationId { get; set; }
}