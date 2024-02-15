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

        if (nationalities == null) return NotFound();

        return Ok(nationalities);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetNationality(string id)
    {
        var nationality = await _nationalityRepository.GetNationalityById(id);

        if (nationality == null) return NotFound("No nationalitywith this id");

        return Ok(nationality);
    }

    // Issues when passing empty data
    [AllowAnonymous]
    [HttpGet("GetByName")]
    public async Task<IActionResult> GetNationalityByName(string name)
    {
        if (string.IsNullOrEmpty(name)) return BadRequest("You should insert a nationality");

        var nation = await _nationalityRepository.GetNationalityByName(name);

        if (nation == null) return BadRequest();

        return Ok(nation);  
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

        return Ok("Nationality added");
    }    


    [HttpDelete]
    public async Task<IActionResult> DeleteNationality(string id)
    {
        var nationalityExists = await _nationalityRepository.NationalityExists(id);

        if (!nationalityExists) return BadRequest("Nationality with this id does not exist");

        var deleteResult = await _nationalityRepository.RemoveNationality(id);

        if (!deleteResult) return BadRequest("Error while deleting the nationality");

        return Ok("Nationality removed");
    }
}