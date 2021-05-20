using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapperService _mapper;

        public CategoriesController(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IMapperService mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _categoryRepository.GetCategories();
            var categoryDtos = categories.Select(c => _mapper.MapCategoryToCategoryDto(c)).ToList();
            return Ok(categoryDtos);
        }

        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(Category category)
        {
            await _categoryRepository.CreateCategoryAsync(category);
            await _unitOfWork.CompleteAsync();
            return Ok(category);
        }

        [HttpPut]
        public async Task<ActionResult<Category>> UpdateCategory(Category category)
        {
            _categoryRepository.UpdateCategory(category);
            await _unitOfWork.CompleteAsync();
            return Ok(category);
        }

        [HttpDelete("{categoryId}")]
        public async Task<ActionResult> DeleteCategory(int categoryId)
        {
            await _categoryRepository.DeleteCategory(categoryId);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }
    }
}