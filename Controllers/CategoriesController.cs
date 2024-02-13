using API.DTO;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoriesController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _categoryRepository.GetAllCategories();

        if (categories == null) return NotFound();

        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(string id)
    {
        var category = await _categoryRepository.GetCategoryById(id);

        if (category == null) return NotFound("Category with this id not found");

        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory(AddCategoryDto categoryDto)
    {
        Category newCategory = new Category 
        {
            Name = categoryDto.Name
        };

        await _categoryRepository.AddCategory(newCategory);

        return Ok("Category added");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCategory(string id)
    {
        var categoryExists = await _categoryRepository.CategoryExists(id);

        if (!categoryExists) return BadRequest("Category with this Id does not exist");

        var deleteResult = await _categoryRepository.RemoveCategory(id);

        if (!deleteResult) return BadRequest("Error while deleting the category");

        return Ok("Category removed successfully");
    }
}