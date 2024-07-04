using API.DTO;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoriesController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _categoryRepository.GetAllCategories();

        if (categories == null) return NotFound();

        return Ok(categories);
        // throw new Exception();
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(string id)
    {
        var category = await _categoryRepository.GetCategoryById(id);

        // if (category == null) return NotFound("Category with this id not found");
        if (category == null) { throw new KeyNotFoundException("Category"); }

        // return Ok(category);
        var response = new ApiResponse
        {
            Result = category,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Error = null
        };
        return Ok(response);
    }

    [AllowAnonymous]
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