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

        if (categories == null) { throw new KeyNotFoundException("Categories"); }

        var response = new ApiResponse
        {
            Result = categories,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Error = null
        };
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(string id)
    {
        var category = await _categoryRepository.GetCategoryById(id);

        if (category == null) { throw new KeyNotFoundException("Category"); }

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

        var response = new ApiResponse
        {
            Result = null,
            IsSuccess = true,
            StatusCode = StatusCodes.Status201Created,
            Error = null
        };
        return Ok(response);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(string id)
    {
        var categoryExists = await _categoryRepository.CategoryExists(id);

        if (!categoryExists) { throw new KeyNotFoundException("Category"); }

        var deleteResult = await _categoryRepository.RemoveCategory(id);

        if (!deleteResult) { throw new BadHttpRequestException("Could not delete category"); }

        var response = new ApiResponse
        {
            Result = null,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Error = null
        };
        return Ok(response);
    }
}