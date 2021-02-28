using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class SliderRepository : ISliderRepository
    {
        private readonly StoreContext _context;
        public SliderRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<SliderItem> CreateSliderItem(SliderItem sliderItem)
        {
            await _context.SliderItems.AddAsync(sliderItem);
            return sliderItem;
        }

        public async Task<SliderItem> GetSliderItemById(int sliderId)
        {
            return await _context.SliderItems.FirstOrDefaultAsync(s => s.Id == sliderId);
        }

        public async Task<IEnumerable<SliderItem>> GetSliderItems()
        {
            return await _context.SliderItems
                .Include(s => s.Photo)
                .ToArrayAsync();
        }

        public SliderItem UpdateSliderItem(SliderItem sliderItem)
        {
            _context.SliderItems.Update(sliderItem);
            return sliderItem;
        }
    }
}