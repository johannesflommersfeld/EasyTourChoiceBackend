using EasyTourChoice.API.Entities;

namespace EasyTourChoice.API.Test.Entities;

public class LocationTest
{
    [TestCase(160.0, -51.0)]
    [TestCase(-160.0, 51.0)]
    [TestCase(160.0, 51.0, 1_000)]
    [TestCase(160.0, 51.0, -10)]
    public void SetValidLocation_CorrectValuesSet(double latitude, double longitude, double? altitude = null)
    {
        // arrange, act
        var location = new Location(latitude, longitude, altitude);

        // assert
        Assert.Multiple(() =>
        {
            Assert.That(location.Latitude, Is.EqualTo(latitude).Within(Tolerances.DOUBLE_EPS * Math.Abs(latitude)));
            Assert.That(location.Longitude, Is.EqualTo(longitude).Within(Tolerances.DOUBLE_EPS * Math.Abs(longitude)));
        });
        if (altitude != null)
        {
            Assert.That(location.Altitude,
                        Is.EqualTo(altitude).Within(Tolerances.DOUBLE_EPS * Math.Abs((double) altitude)));
        }
    }

    [Test]
    public void SetNullLocation_CoordinatesAreNulled()
    {
        // arrange
        double? latitude = null;
        double? longitude = null;

        // act
        var location = new Location(latitude, longitude);

        // assert
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
    public void SetOutOfRangeLocation_ThrowsArgumentException(double latitude, double longitude, double? altitude = null)
    {
        Assert.That(() => new Location(latitude, longitude, altitude),
            Throws.TypeOf<ArgumentException>()
                .With.Message.EqualTo("Longitude has to be in the range [-90, 90], latitude has to be in the range [-180, 180], altitude has to be in the range [-450, 8900]."));
    }

    [TestCase(double.NaN, 1)]
    [TestCase(1, double.NaN)]
    [TestCase(1, 1, double.NaN)]
    public void SetInvalidLocation_ThrowsArgumentException(double latitude, double longitude, double? altitude = null)
    {
        Assert.That(() => new Location(latitude, longitude, altitude),
            Throws.TypeOf<ArgumentException>()
                .With.Message.EqualTo("Nan is not a valid coordinate."));
    }
}