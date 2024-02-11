using API.Configurations;
using API.DTO;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookRepository _bookRepository;

    // private readonly IMongoCollection<Book> _bookRepository;
    public BookController(
        // IMongoDatabase mongoDatabase,
        // IOptions<DatabaseSettings> dbSettings
        IBookRepository bookRepository
    )
    {
        _bookRepository = bookRepository;
        // _bookRepository = mongoDatabase.GetCollection<Book>(dbSettings.Value.BookCollectionName);    
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBooks()
    {
        // var books = await _bookRepository.Find(_ => true).ToListAsync();
        var books = await _bookRepository.GetAllBooks();

        if (books == null) return NotFound("No books yet :/");

        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBook(string id)
    {
        // var book = await _bookRepository.Find(b => b.Id == id).FirstOrDefaultAsync();
        var book = await _bookRepository.GetBookById(id);

        return Ok(book);
    }

    // [HttpGet("GetByName")]
    // public async Task<IActionResult> GetBookByName(string bookName)
    // {
    //     string bookNameLower = bookName.ToLower();

    //     var book = await _bookRepository.Find(b => b.Title.ToLower() == bookNameLower).FirstOrDefaultAsync();

    //     return Ok(book);
    // }

    [HttpPost]
    public async Task<IActionResult> AddBook(AddBookDto bookDto)
    {
        if (bookDto == null)
        {
            return BadRequest();
        }

        Book newBook = new Book
        {
            Title = bookDto.Title,
            Description = bookDto.Description,
            PublicationDate = new DateTime(bookDto.PublicationYear, 1, 1)
        };

        // await _bookRepository.InsertOneAsync(newBook);
        await _bookRepository.AddAsync(newBook);

        return Ok(newBook);
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveBook(string bookId)
    {
        // Book bookToDelete = await _bookRepository.Find(b => b.Id == bookId).FirstOrDefaultAsync();
        bool bookExists = await _bookRepository.BookExists(bookId);

        if (!bookExists) return NotFound();

        // var deleteResult = await _bookRepository.DeleteOneAsync(b => b.Id == bookId);
        bool deleteResult = await _bookRepository.RemoveAsync(bookId);

        if (!deleteResult)
        {
            return BadRequest("Not deleted");
        }

        return Ok("Book deleted");
    }
}