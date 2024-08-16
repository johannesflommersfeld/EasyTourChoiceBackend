using EasyTourChoice.API.Entities;

namespace EasyTourChoice.API.Models;

public class Tour(
    string name, Activity activity, Location? startingLocation, Location? activityLocation, float? duration,
    float? approachDuration, string? shortDescription, int? difficulty, int? risk, Area? area)
{
    public int Id { get; set; }
    public required string Name { get; set; } = name;
    public Activity ActivityType { get; set; } = activity;
    public Location? StartingLocation { get; set; } = startingLocation;
    public Location? ActivityLocation { get; set; } = activityLocation;
    public float? Duration { get; set; } = duration; // expected activity time in hours
    public float? ApproachDuration { get; set; } = approachDuration; // expected approach time in hours
    public string? ShortDescription { get; set; } = shortDescription;
    public int? Difficulty { get; set; } = difficulty; // unit depends on the type of activity
    public int? Risk { get; set; } = risk; // categories depend on the type of activity
    public Area? Area { get; set; } = area;
}