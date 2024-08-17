using EasyTourChoice.API.Entities;
using EasyTourChoice.API.ValidationAttributes;

namespace EasyTourChoice.API.Test.ValidationAttributes;

public class LocationAttributeTest
{
    private LocationAttribute _locationAttribute;

    [SetUp]
    public void SetUp()
    {
        _locationAttribute = new LocationAttribute();
    }

    [TestCase(160.0, -51.0)]
    [TestCase(-160.0, 51.0)]
    [TestCase(160.0, 51.0, 1_000)]
    [TestCase(160.0, 51.0, -10)]
    public void SetValidLocation_CorrectValuesSet(double latitude, double longitude, double? altitude = null)
    {
        // arrange
        var location = new Location() {Latitude = latitude, Longitude = longitude, Altitude = altitude};
        
        // act, assert
        Assert.That(_locationAttribute.IsValid(location), Is.True);
        Assert.Multiple(() =>
        {
            Assert.That(location.Latitude,
                        Is.EqualTo(latitude).Within(Tolerances.DOUBLE_EPS * Math.Abs(latitude)));
            Assert.That(location.Longitude,
                        Is.EqualTo(longitude).Within(Tolerances.DOUBLE_EPS * Math.Abs(longitude)));
        });
        if (altitude != null)
        {
            Assert.That(location.Altitude,
                        Is.EqualTo(altitude).Within(Tolerances.DOUBLE_EPS * Math.Abs((double)altitude)));
        }
    }

    [Test]
    public void SetNullLocation_CoordinatesAreNulled()
    {
        // arrange
        var location = new Location();

        // act, assert
        Assert.That(_locationAttribute.IsValid(location), Is.True);
        Assert.Multiple(() =>
        {
            Assert.That(location.Latitude, Is.Null);
            Assert.That(location.Longitude, Is.Null);
            Assert.That(location.Altitude, Is.Null);
        });
    }

    [TestCase(182, -91)]
    [TestCase(-182, 91)]
    [TestCase(182, 1)]
    [TestCase(1, 91)]
    [TestCase(1, double.PositiveInfinity)]
    [TestCase(1, double.NegativeInfinity)]
    [TestCase(double.PositiveInfinity, 1)]
    [TestCase(double.NegativeInfinity, 1)]
    [TestCase(-160, 51, 10000)]
    [TestCase(-160, 51, -500)]
    [TestCase(double.NaN, 1)]
    [TestCase(1, double.NaN)]
    [TestCase(1, 1, double.NaN)]
    public void SetInvalidLocation_IsValidReturnsFalse(double latitude, double longitude, double? altitude = null)
    {
        // arrange
        var location = new Location() {Latitude = latitude, Longitude = longitude, Altitude = altitude};

        // act, assert
        Assert.That(_locationAttribute.IsValid(location), Is.False);
    }
}