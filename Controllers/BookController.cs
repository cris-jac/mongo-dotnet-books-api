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
    private readonly IAuthorRepository _authorRepository;

    // private readonly IMongoCollection<Book> _bookRepository;
    public BookController(
        // IMongoDatabase mongoDatabase,
        // IOptions<DatabaseSettings> dbSettings
        IBookRepository bookRepository,
        IAuthorRepository authorRepository
    )
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
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

        if (book == null) return NotFound("There are no books with this Id");

        // Parse authors Ids to strings
        List<string> authorsStringIds = book.AuthorIds.ConvertAll(id => id.ToString());
        var authors = await _authorRepository.GetAuthorsByIds(authorsStringIds);

        // Response Dto
        GetBookDto bookDto = new GetBookDto
        {
            Id = book.Id,
            Title = book.Title,
            Description = book.Description,
            Authors = authors ?? new List<string>()
        };

        return Ok(bookDto);
    }

    [HttpGet("GetByAuthor")]
    public async Task<IActionResult> GetBooksByAuthor(string authorId)
    {
        var authorExists = await _authorRepository.AuthorExists(authorId);

        if (!authorExists) return NotFound("Author with this Id does not exist");

        var books = await _bookRepository.GetBooksByAuthor(authorId);

        if (books == null) return NotFound("Error: Not found");

        if (books.Count() == 0) return Ok("There are no books here");

        return Ok(books);
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

    [HttpPut("AddAuthorToBook")]
    public async Task<IActionResult> AddAuthorToBook(string bookId, string authorId)
    {
        var bookExists = await _bookRepository.BookExists(bookId);

        if (!bookExists) return NotFound("There is no book with this Id");

        var authorExists = await _authorRepository.AuthorExists(authorId);

        if (!authorExists) return NotFound("There is no author with this Id");
        
        var authorAddedToBook = await _bookRepository.AddAuthorToBook(bookId, authorId);

        if (!authorAddedToBook) return BadRequest("Error adding author to book");

        return Ok("Book's author updated");
    }

    [HttpPut("RemoveAuthorFromBook")]
    public async Task<IActionResult> RemoveAuthorFromBook(string bookId, string authorId)
    {
        var bookExists = await _bookRepository.BookExists(bookId);

        if (!bookExists) return NotFound("There is no book with this Id");

        var authorExists = await _authorRepository.AuthorExists(authorId);

        if (!authorExists) return NotFound("There is no author with this Id");
        
        var authorRemovedFromBook = await _bookRepository.RemoveAuthorFromBook(bookId, authorId);

        if (!authorRemovedFromBook) return BadRequest("Error removing author from book");

        return Ok("Book's author updated");
    }
    
}