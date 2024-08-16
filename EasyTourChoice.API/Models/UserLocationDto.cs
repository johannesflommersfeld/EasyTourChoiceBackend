using EasyTourChoice.API.Entities;

namespace EasyTourChoice.API.Models;

public class UserLocationDto(double? latitude, double? longitude, double? altitude) : 
    Location(latitude, longitude, altitude);