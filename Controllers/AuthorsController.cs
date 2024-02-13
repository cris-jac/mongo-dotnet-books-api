using API.Interfaces;
using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace API.DTO;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorRepository _authorRepository;
    private readonly INationalityRepository _nationalityRepository;

    public AuthorsController(
        IAuthorRepository authorRepository,
        INationalityRepository nationalityRepository
    )
    {
        _authorRepository = authorRepository;
        _nationalityRepository = nationalityRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAuthors()
    {
        var authors = await _authorRepository.GetAllAuthors();

        if (authors == null) return NotFound("No authors yet");

        // var authorsDtoTasks = authors.Select(async author => 
        // {
        //     var nationality = await _nationalityRepository.GetNationalityById(author.NationalityId.ToString());

        //     return new GetAuthorDto
        //     {
        //         Id = author.Id.ToString(),
        //         Name = author.Name,
        //         Bio = author.Bio,
        //         Nationality = nationality?.Name ?? ""
        //     };
        // });

        // var authorsDto = await Task.WhenAll(authorsDtoTasks);

        var authorsDto = new List<GetAuthorDto>();

        foreach (var author in authors)
        {
            var nationality = await _nationalityRepository.GetNationalityById(author.NationalityId.ToString());

            authorsDto.Add(new GetAuthorDto 
            {
                Id = author.Id.ToString(),
                Name = author.Name,
                Bio = author.Bio,
                Nationality = nationality?.Name ?? ""
            });
        }

        return Ok(authorsDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAuthor(string id)
    {
        var author = await _authorRepository.GetAuthorById(id);

        if (author == null) return NotFound("There are no authors with this Id");

        var nationality = await _nationalityRepository.GetNationalityById(author.NationalityId.ToString());

        var authorDto = new GetAuthorDto 
        {
            Id = author.Id.ToString(),
            Name = author.Name,
            Bio = author.Bio,
            // Nationality = (nationality!=null) ? nationality.Name : ""
            Nationality = nationality.Name ?? ""
        };

        return Ok(authorDto);
    }

    [HttpGet("GetByCountry")]
    public async Task<IActionResult> GetAuthorsByNationality(string nationalityId)
    {
        // Check if nationality exists
        var nationalityExists = await _nationalityRepository.NationalityExists(nationalityId);

        if (!nationalityExists) return NotFound("Nationality does not exist");

        // Check if authors with this nationality exist
        var authors = await _authorRepository.GetAuthorsByNationalityId(nationalityId);

        if (authors == null) return NotFound("There are no authors with this nationality");

        // Map authors response
        var nationality = await _nationalityRepository.GetNationalityById(nationalityId);
        var authorsDto = new List<GetAuthorDto>();

        foreach (var author in authors)
        {
            // var nationality = await _nationalityRepository.GetNationalityById(author.NationalityId.ToString());

            authorsDto.Add(new GetAuthorDto 
            {
                Id = author.Id.ToString(),
                Name = author.Name,
                Bio = author.Bio,
                Nationality = nationality.Name
            });
        }

        return Ok(authorsDto);
    }

    [HttpPost]
    public async Task<IActionResult> AddAuthor(AddAuthorDto authorDto)
    {
        Author newAuthor = new Author
        {
            Name = authorDto.Name,
            Bio = authorDto.Bio,
            NationalityId = ObjectId.Empty
        };

        await _authorRepository.AddAuthor(newAuthor);

        return Ok("Author added");
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveAuthor(string id)
    {
        var authorExists = await _authorRepository.AuthorExists(id);

        if (!authorExists) return NotFound("No author with this Id");
        
        var deleteResult = await _authorRepository.RemoveAsync(id);

        if (!deleteResult) return BadRequest("Error: Author not deleted");

        return Ok("Author deleted");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAuthor(string id, UpdateAuthorDto authorDto)
    {
        var authorExists = await _authorRepository.AuthorExists(id);

        if (!authorExists) return NotFound("Author not found");

        Author authorToReplace = new Author
        {
            Name = authorDto.Name,
            Bio = authorDto.Bio,
            NationalityId = ObjectId.Parse(authorDto.NationalityId)
        };

        await _authorRepository.UpdateAuthor(id, authorToReplace);

        return Ok("Author updated");
    }

    [HttpPut("AddNationalityId")]
    public async Task<IActionResult> AddNationalityToAuthor(string authorId, string nationalityId)
    {
        var author = await _authorRepository.GetAuthorById(authorId);

        if (author == null) return NotFound("Not author found with this Id");

        var nationalityExists = await _nationalityRepository.NationalityExists(nationalityId);

        if (!nationalityExists) return NotFound("Nationality does not exist");

        // author.NationalityId = ObjectId.Parse(nationalityId);

        // var updateDefinition = Builders<Author>.Update.Set(a => a.NationalityId, ObjectId.Parse(nationalityId));
        // var updateResult = await _authorRepository.

        var updateResult = await _authorRepository.UpdateAuthorNationality(authorId, nationalityId);

        if (!updateResult) return BadRequest("Error while updateing the author's nationality");

        return Ok("Author's nationality updated");
    }
}