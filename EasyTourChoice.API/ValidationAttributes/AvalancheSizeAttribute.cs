using System.ComponentModel.DataAnnotations;

namespace EasyTourChoice.API.ValidationAttributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
sealed public class AvalancheSizeAttribute : ValidationAttribute
{
    private const double _minSize = 1;
    private const double _maxSize = 5;

    public override bool IsValid(object? value)
    {
        if (value == null)
            return false;

        var avalancheSize = (int)value;

        if (avalancheSize < _minSize || avalancheSize > _maxSize)
        {
            return false;
        }

        return true;
    }

    public override string FormatErrorMessage(string name)
    {
        var msg = $"Avalanche size has to be in the range [{_minSize}, {_maxSize}]";
        return msg;
    }
}