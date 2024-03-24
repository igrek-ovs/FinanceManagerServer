using FinancialAccountingServer.DTOs;
using FinancialAccountingServer.Services.interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FinancialAccountingServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryDTO categoryDTO)
        {
            try
            {
                await _categoryService.AddCategory(categoryDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding the category: {ex.Message}");
            }
        }

        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetCategoriesForGroup(int groupId)
        {
            try
            {
                var categories = await _categoryService.GetCategoriesForGroup(groupId);
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving categories: {ex.Message}");
            }
        }

        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> RemoveCategory(int categoryId)
        {
            try
            {
                var result = await _categoryService.RemoveCategory(categoryId);
                if (result)
                {
                    return Ok();
                }
                else
                {
                    return NotFound($"Category with ID {categoryId} not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while removing the category: {ex.Message}");
            }
        }
    }
}
