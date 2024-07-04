using API.Interfaces;
using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace API.DTO;

[ApiController]
[Authorize]
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

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllAuthors()
    {
        var authors = await _authorRepository.GetAllAuthors();

        if (authors == null) { throw new KeyNotFoundException("Authors"); }

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

        var response = new ApiResponse
        {
            Result = authorsDto,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Error = null
        };
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAuthor(string id)
    {
        var author = await _authorRepository.GetAuthorById(id);

        if (author == null) { throw new KeyNotFoundException("Author"); }

        var nationality = await _nationalityRepository.GetNationalityById(author.NationalityId.ToString());

        var authorDto = new GetAuthorDto
        {
            Id = author.Id.ToString(),
            Name = author.Name,
            Bio = author.Bio,
            Nationality = nationality.Name ?? ""
        };

        var response = new ApiResponse
        {
            Result = authorDto,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Error = null
        };
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("nationality/{nationalityId}")]
    public async Task<IActionResult> GetAuthorsByNationality(string nationalityId)
    {
        // Check if nationality exists
        var nationalityExists = await _nationalityRepository.NationalityExists(nationalityId);

        if (!nationalityExists) { throw new KeyNotFoundException("Nationality"); }

        // Check if authors with this nationality exist
        var authors = await _authorRepository.GetAuthorsByNationalityId(nationalityId);

        if (authors == null) { throw new KeyNotFoundException("Authors"); }

        // Map authors response
        var nationality = await _nationalityRepository.GetNationalityById(nationalityId);
        var authorsDto = new List<GetAuthorDto>();

        foreach (var author in authors)
        {
            authorsDto.Add(new GetAuthorDto
            {
                Id = author.Id.ToString(),
                Name = author.Name,
                Bio = author.Bio,
                Nationality = nationality.Name
            });
        }

        var response = new ApiResponse
        {
            Result = authorsDto,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Error = null
        };
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> AddAuthor(AddAuthorDto authorDto)
    {
        Author newAuthor = new Author
        {
            Name = authorDto.Name,
            Bio = authorDto.Bio,
            NationalityId = string.IsNullOrEmpty(authorDto.NationalityId) ? ObjectId.Empty : ObjectId.Parse(authorDto.NationalityId)
        };

        await _authorRepository.AddAuthor(newAuthor);

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
    public async Task<IActionResult> RemoveAuthor([FromRoute] string id)
    {
        var authorExists = await _authorRepository.AuthorExists(id);

        if (!authorExists) { throw new KeyNotFoundException("Author"); }

        var deleteResult = await _authorRepository.RemoveAuthor(id);

        if (!deleteResult) { throw new BadHttpRequestException("Could not delete author"); }

        var response = new ApiResponse
        {
            Result = null,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Error = null
        };
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAuthor([FromRoute] string id, UpdateAuthorDto authorDto)
    {
        var authorExists = await _authorRepository.AuthorExists(id);

        if (!authorExists) { throw new KeyNotFoundException("Author"); }

        Author authorToReplace = new Author
        {
            Name = authorDto.Name,
            Bio = authorDto.Bio,
            NationalityId = ObjectId.Parse(authorDto.NationalityId)
        };

        await _authorRepository.UpdateAuthor(id, authorToReplace);

        var response = new ApiResponse
        {
            Result = null,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Error = null
        };
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPut("{authorId}/{nationalityId}")]
    public async Task<IActionResult> AddNationalityToAuthor([FromRoute] string authorId, [FromRoute] string nationalityId)
    {
        var author = await _authorRepository.GetAuthorById(authorId);

        if (author == null) { throw new KeyNotFoundException("Author"); }

        var nationalityExists = await _nationalityRepository.NationalityExists(nationalityId);

        if (!nationalityExists) { throw new KeyNotFoundException("Nationality"); }

        var updateResult = await _authorRepository.UpdateAuthorNationality(authorId, nationalityId);

        if (!updateResult) { throw new BadHttpRequestException("Could not update the author"); }

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