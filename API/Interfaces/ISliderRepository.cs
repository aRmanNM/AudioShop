using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface ISliderRepository
    {
        Task<IEnumerable<SliderItem>> GetSliderItemsAsync();
        Task<SliderItem> GetSliderItemByIdAsync(int sliderId);
        Task<SliderItem> CreateSliderItemAsync(SliderItem sliderItem);
        SliderItem UpdateSliderItem(SliderItem sliderItem);
        void DeleteSliderItem(SliderItem sliderItem);
    }
}