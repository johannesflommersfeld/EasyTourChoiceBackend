using EasyTourChoice.API.Entities;

namespace EasyTourChoice.API.Services;

public interface ITourDataRepository
{
    IEnumerable<TourData> GetAll();
    IEnumerable<TourData> GetAllByActivity(Activity activity);
    TourData GetTourData(int id);
}

