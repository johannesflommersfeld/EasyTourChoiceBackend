namespace EasyTourChoice.API.Application.Models;


public record DangerRatingDto
{
    required public AvalancheDangerRating MainValue { get; set; }

    public string? UpperBound { get; set; }

    public string? LowerBound { get; set; }

    required public ValidTimePeriod ValidTimePeriod { get; set; }
}