using API.DTO;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class NationalitiesController : ControllerBase
{
    private readonly INationalityRepository _nationalityRepository;

    public NationalitiesController(INationalityRepository nationalityRepository)
    {
        _nationalityRepository = nationalityRepository;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllNationalities()
    {
        var nationalities = await _nationalityRepository.GetAllNationalities();

        if (nationalities == null) { throw new KeyNotFoundException("Nationalities"); }

        var response = new ApiResponse
        {
            Result = nationalities,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Error = null
        };
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetNationality(string id)
    {
        var nationality = await _nationalityRepository.GetNationalityById(id);

        if (nationality == null) { throw new KeyNotFoundException("Nationality"); }

        var response = new ApiResponse
        {
            Result = nationality,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Error = null
        };
        return Ok(response);
    }

    // Issues when passing empty data
    [AllowAnonymous]
    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetNationalityByName(string name)
    {
        if (string.IsNullOrEmpty(name)) { throw new BadHttpRequestException("Invalid input"); }

        var nation = await _nationalityRepository.GetNationalityByName(name);

        if (nation == null) { throw new KeyNotFoundException("Nationality"); }

        var response = new ApiResponse
        {
            Result = nation,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Error = null
        };
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> AddNationality(AddNationalityDto nationalityDto)
    {
        Nationality newNationality = new Nationality
        {
            Name = nationalityDto.Name
        };

        await _nationalityRepository.AddNationality(newNationality);

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
    public async Task<IActionResult> DeleteNationality(string id)
    {
        var nationalityExists = await _nationalityRepository.NationalityExists(id);

        if (!nationalityExists) { throw new KeyNotFoundException("Nationality"); }

        var deleteResult = await _nationalityRepository.RemoveNationality(id);

        if (!deleteResult) { throw new KeyNotFoundException("Could not delete nationality"); }

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