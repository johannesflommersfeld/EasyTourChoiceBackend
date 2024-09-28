using System.ComponentModel.DataAnnotations;
using EasyTourChoice.API.Entities;

namespace EasyTourChoice.API.ValidationAttributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
sealed public class LocationAttribute : ValidationAttribute
{
    private const double _minLatitude = -180;
    private const double _maxLatitude = 180;
    private const double _minLongitude = -90;
    private const double _maxLongitude = 90;
    private const double _minAltitude = -450;
    private const double _maxAltitude = 8_900;

    public override bool IsValid(object? value)
    {
        if (value == null)
            return false;

        var location = (Location)value;
        if (double.IsNaN((double)location.Latitude) ||
            double.IsNaN((double)location.Longitude) ||
            (location.Altitude != null && double.IsNaN((double)location.Altitude)))
        {
            return false;
        }

        if (location.Latitude < _minLatitude || location.Latitude > _maxLatitude ||
            location.Longitude < _minLongitude || location.Longitude > _maxLongitude ||
            location.Altitude < _minAltitude || location.Altitude > _maxAltitude)
        {
            return false;
        }

        return true;
    }

    public override string FormatErrorMessage(string name)
    {
        var msg = string.Format("Longitude, latitude and altitude have to be valid floating point numbers in the ranges "
                + " [{0}, {1}], [{2}, {3}], and [{4}, {5}], respectively.",
                _minLongitude, _maxLongitude, _minLatitude, _maxLatitude, _minAltitude, _maxAltitude);
        return msg;
    }
}