using API.DTO;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace  API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ReadersController : ControllerBase
{
    private readonly IReaderRepository _readerRepository;
    private readonly IBookRepository _bookRepository;
    private readonly INationalityRepository _nationalityRepository;

    public ReadersController(
        IReaderRepository readerRepository,
        IBookRepository bookRepository,
        INationalityRepository nationalityRepository
    )
    {
        _readerRepository = readerRepository;
        _bookRepository = bookRepository;
        _nationalityRepository = nationalityRepository;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllReaders()
    {
        var readers = await _readerRepository.GetAllAsync();

        if (readers ==  null)
        {
            return NotFound("No readers yet");
        }

        // List of reades
        var readersDto = new List<GetReaderDto>();

        foreach (var reader in readers)
        {
            // get nationality name
            var nationality = await _nationalityRepository.GetNationalityById(reader.NationalityId.ToString());

            // get books names
            List<string> booksStringIds = reader.BookIds.ConvertAll(id => id.ToString());
            List<string> books = await _bookRepository.GetBooksByIds(booksStringIds);

            // map
            readersDto.Add(new GetReaderDto
            {
                Id = reader.Id,
                Name = reader.Name,
                Username = reader.Username,
                Nationality = nationality?.Name ?? "",
                Books = books ?? new List<string>()
            });  
        }

        return Ok(readersDto);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetReader(string id)
    {
        var reader = await _readerRepository.GetReaderById(id);
        if (reader == null)
        {
            return NotFound("No reader with this id");
        }

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

        return Ok(readerDto);
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

        return Ok("Reader added");
    }

    [AllowAnonymous]
    [HttpGet("GetReaderBooks")]
    public async Task<IActionResult> GetBooksFromReader(string readerId)
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

        return Ok(books);
    }

    // [HttpPost]
    // public async Task<IActionResult> AddBookToList(string readerId, string bookId)
    // {
    //     Reader reader = await _readerRepository.GetReaderById(readerId);

    //     if (reader == null)
    //     {
    //         return NotFound("No reader with this Id");
    //     }

    //     // Check if the book exists
    //     var bookExists = await _bookRepository.BookExists(bookId);

    //     if (!bookExists) 
    //     {
    //         return NotFound("The book id does not exists");
    //     } 

    //     // Add the bookId
    //     ObjectId bookObjectId = ObjectId.Parse(bookId);
    //     reader.BookIds.Add(bookObjectId);

    //     return Created();
    // }


    // [HttpDelete]
    // public async Task<IActionResult> RemoveBookFromList(string readerId, string bookId)
    // {
    //     Reader reader = await _readerRepository.GetReaderById(readerId);

    //     if (reader == null) 
    //     { 
    //         return NotFound("Reader does not exists");
    //     }

    //     ObjectId bookObjectId = ObjectId.Parse(bookId);
    //     bool readerHasBook = reader.BookIds.Contains(bookObjectId);

    //     if (!readerHasBook)
    //     {
    //         return NotFound("User does not have this book");
    //     }

    //     reader.BookIds.Remove(bookObjectId);

    //     return NoContent();
    // }

    [AllowAnonymous]
    [HttpPut("AddBookToReader")]
    public async Task<IActionResult> AddBookToReader(string readerId, string bookId)
    {
        var readerExists = await _readerRepository.ReaderExists(readerId);
        if (!readerExists) return NotFound("Reader with this Id not found");

        var bookExists = await _bookRepository.BookExists(bookId);
        if (!bookExists) return NotFound("Book with this Id not found");

        var bookAddedToReader = await _readerRepository.AddBookToList(readerId, bookId);
        if (!bookAddedToReader) return BadRequest("Error while adding book to reader");

        return Ok("Reader's list of books was updated");
    }

    [AllowAnonymous]
    [HttpPut("RemoveBookFromReader")]
    public async Task<IActionResult> RemoveBookFromReader(string readerId, string bookId)
    {
        var readerExists = await _readerRepository.ReaderExists(readerId);
        if (!readerExists) return NotFound("Reader with this Id not found");

        var bookExists = await _bookRepository.BookExists(bookId);
        if (!bookExists) return NotFound("Book with this Id not found");

        var bookRemovedFromReader = await _readerRepository.RemoveBookFromList(readerId, bookId);
        if (!bookRemovedFromReader) return BadRequest("Error while removing book to reader");

        return Ok("Reader's list of books was updated");
    }

    [AllowAnonymous]
    [HttpPut("UpdateNationality")]
    public async Task<IActionResult> UpdateNationalityToReader(string readerId, string nationalityId)
    {
        var readerExists = await _readerRepository.ReaderExists(readerId);
        if (!readerExists) return NotFound("Reader with this Id was not found");

        var nationalityExists = await _nationalityRepository.NationalityExists(nationalityId);
        if (!nationalityExists) return NotFound("Nationality with this Id was not found");

        var nationalityUpdated = await _readerRepository.UpdateReaderNationality(readerId, nationalityId);
        if (!nationalityUpdated) return BadRequest("Error while updating nationality");

        return Ok("Reader's nationality updated");
    }
}