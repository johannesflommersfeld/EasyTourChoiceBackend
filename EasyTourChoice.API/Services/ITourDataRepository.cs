using EasyTourChoice.API.Models;

namespace EasyTourChoice.API.Services;

public interface ITourDataRepository
{
    IEnumerable<TourDataDto> GetAll();
    IEnumerable<TourDataDto> GetAllByActivity(Activity activity);

}

