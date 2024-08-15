using EasyTourChoice.API.Models;

namespace EasyTourChoice.API.Services;

public class TourDataRepository : ITourDataRepository
{
        public IEnumerable<TourDataDto> GetAll()
        {
            return [];
        }

        public IEnumerable<TourDataDto> GetAllByActivity(Activity activity)
        {
            return [];
        }

}