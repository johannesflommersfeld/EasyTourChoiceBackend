using System.ComponentModel.DataAnnotations;

namespace EasyTourChoice.API.ValidationAttributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
sealed public class AspectAttribute : ValidationAttribute
{
    const byte _maxAspect = 0b1111_1111;

    public override bool IsValid(object? value)
    {
        if (value == null)
            return false;

        var aspect = (byte)value;
        if (aspect > _maxAspect)
            return false;

        return true;
    }

    public override string FormatErrorMessage(string name)
    {
        var msg = string.Format("The aspect has to be represented by a byte in the range [0, {0}]", _maxAspect);
        return msg;
    }
}