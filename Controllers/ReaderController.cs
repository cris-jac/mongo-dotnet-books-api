using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace  API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReaderController : ControllerBase
{
    private readonly IReaderRepository _readerRepository;
    private readonly IBookRepository _bookRepository;

    public ReaderController(
        IReaderRepository readerRepository,
        IBookRepository bookRepository
    )
    {
        _readerRepository = readerRepository;
        _bookRepository = bookRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllReaders()
    {
        var readers = await _readerRepository.GetAllAsync();

        if (readers ==  null)
        {
            return NotFound("No readers yet");
        }

        return Ok(readers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetReader(string id)
    {
        var reader = await _readerRepository.GetReaderById(id);
        
        if (reader == null)
        {
            return NotFound("No reader with this id");
        }

        return Ok(reader);
    }

    // [HttpPost]
    // public async Task<IActionResult> AddReader(Reader reader)
    // {
    // }

    [HttpGet("readerBooks")]
    public async Task<IActionResult> GetBooksFromReader(string readerId)
    {
        var reader = await _readerRepository.GetReaderById(readerId);

        // Pass the list of ObjectIds to string
        var bookIds = reader.BookIds.Select(id => id.ToString()).ToList();

        // Empty array to contain the books
        var books = new List<object>();

        // Get the book and add it to the list
        foreach (var bookId in bookIds) 
        {
            // bookId = bookId.ToString();
            var book = await _bookRepository.GetBookById(bookId);
            books.Add(book);
        }

        return Ok(books);
    }

    [HttpPost]
    public async Task<IActionResult> AddBookToList(string readerId, string bookId)
    {
        Reader reader = await _readerRepository.GetReaderById(readerId);

        if (reader == null)
        {
            return NotFound("No reader with this Id");
        }

        // Check if the book exists
        var bookExists = await _bookRepository.BookExists(bookId);

        if (!bookExists) 
        {
            return NotFound("The book id does not exists");
        } 

        // Add the bookId
        ObjectId bookObjectId = ObjectId.Parse(bookId);
        reader.BookIds.Add(bookObjectId);

        return Created();
    }


    [HttpDelete]
    public async Task<IActionResult> RemoveBookFromList(string readerId, string bookId)
    {
        Reader reader = await _readerRepository.GetReaderById(readerId);

        if (reader == null) 
        { 
            return NotFound("Reader does not exists");
        }

        ObjectId bookObjectId = ObjectId.Parse(bookId);
        bool readerHasBook = reader.BookIds.Contains(bookObjectId);

        if (!readerHasBook)
        {
            return NotFound("User does not have this book");
        }

        reader.BookIds.Remove(bookObjectId);

        return NoContent();
    }

}