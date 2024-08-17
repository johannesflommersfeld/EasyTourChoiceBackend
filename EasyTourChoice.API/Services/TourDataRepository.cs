using EasyTourChoice.API.Entities;

namespace EasyTourChoice.API.Services;

public class TourDataRepository : ITourDataRepository
{
    public IEnumerable<TourData> GetAll()
    {
        return [];
    }

    public IEnumerable<TourData> GetAllByActivity(Activity activity)
    {
        return [];
    }

    public TourData GetTourData(int id)
    {
        var tourData = new TourData() { Name = string.Empty};
        return tourData;
    }

}