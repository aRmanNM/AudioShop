using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface ISliderRepository
    {
        Task<IEnumerable<SliderItem>> GetSliderItems();
        Task<SliderItem> GetSliderItemById(int sliderId);
        Task<SliderItem> CreateSliderItem(SliderItem sliderItem);
        SliderItem UpdateSliderItem(SliderItem sliderItem);
    }
}