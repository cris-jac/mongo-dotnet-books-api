using API.DTO;
using API.Interfaces;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ReadersController : ControllerBase
{
    private readonly IReaderRepository _readerRepository;
    private readonly IBookRepository _bookRepository;
    private readonly INationalityRepository _nationalityRepository;
    private readonly ReaderService _readerService;

    public ReadersController(
        IReaderRepository readerRepository,
        IBookRepository bookRepository,
        INationalityRepository nationalityRepository,
        ReaderService readerService
    )
    {
        _readerRepository = readerRepository;
        _bookRepository = bookRepository;
        _nationalityRepository = nationalityRepository;
        _readerService = readerService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllReaders()
    {
        var readers = await _readerRepository.GetAllAsync();

        if (readers == null) { throw new KeyNotFoundException("Readers"); }

        var readersDto = await _readerService.MapReaders(readers);

        var response = new ApiResponse
        {
            Result = readersDto,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Error = null
        };
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetReader(string id)
    {
        var reader = await _readerRepository.GetReaderById(id);
        if (reader == null) { throw new KeyNotFoundException("Reader"); }

        // get nationality name
        var nationality = await _nationalityRepository.GetNationalityById(reader.NationalityId.ToString());

        // get books names
        List<string> booksStringIds = reader.BookIds.ConvertAll(id => id.ToString());
        List<string> books = await _bookRepository.GetBooksByIds(booksStringIds);

        // map
        GetReaderDto readerDto = new GetReaderDto
        {
            Id = reader.Id,
            Name = reader.Name,
            Username = reader.Username,
            Nationality = nationality?.Name ?? "",
            Books = books ?? new List<string>()
        };

        var response = new ApiResponse
        {
            Result = readerDto,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Error = null
        };
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> AddReader(AddReaderDto readerDto)
    {
        Reader newReader = new Reader
        {
            Name = readerDto.Name,
            Username = readerDto.Username,
            NationalityId = ObjectId.Empty,
            BookIds = new List<ObjectId>()
        };

        await _readerRepository.AddReader(newReader);

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
    [HttpGet("reader/{readerId}")]
    public async Task<IActionResult> GetBooksFromReader([FromRoute] string readerId)
    {
        var reader = await _readerRepository.GetReaderById(readerId);

        // Pass the list of ObjectIds to string
        var bookIds = reader.BookIds.Select(id => id.ToString()).ToList();

        // Empty array to contain the books
        List<GetReaderBookDto> books = new List<GetReaderBookDto>();

        // Get the book and add it to the list
        foreach (var bookId in bookIds)
        {
            // bookId = bookId.ToString();
            var book = await _bookRepository.GetBookById(bookId);
            books.Add(new GetReaderBookDto
            {
                Id = book.Id,
                Title = book.Title
            });
        }

        var response = new ApiResponse
        {
            Result = books,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Error = null
        };
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPut("{readerId}/book/{bookId}")]
    public async Task<IActionResult> AddBookToReader([FromRoute] string readerId, [FromRoute] string bookId)
    {
        var readerExists = await _readerRepository.ReaderExists(readerId);
        if (!readerExists) { throw new KeyNotFoundException("Reader"); }

        var bookExists = await _bookRepository.BookExists(bookId);
        if (!bookExists) { throw new KeyNotFoundException("Book"); }

        var bookAddedToReader = await _readerRepository.AddRemoveBookToList(readerId, bookId);
        if (!bookAddedToReader) { throw new BadHttpRequestException("Could not update reader"); }

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
    [HttpPut("{readerId}/nationality/{nationalityId}")]
    public async Task<IActionResult> UpdateNationalityToReader([FromRoute] string readerId, [FromRoute] string nationalityId)
    {
        var readerExists = await _readerRepository.ReaderExists(readerId);
        if (!readerExists) { throw new KeyNotFoundException("Reader"); }

        var nationalityExists = await _nationalityRepository.NationalityExists(nationalityId);
        if (!nationalityExists) { throw new KeyNotFoundException("Nationality"); }

        var nationalityUpdated = await _readerRepository.UpdateReaderNationality(readerId, nationalityId);
        if (!nationalityUpdated) { throw new KeyNotFoundException("Could not update reader"); }

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