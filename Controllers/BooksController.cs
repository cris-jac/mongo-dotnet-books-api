using API.Configurations;
using API.DTO;
using API.Interfaces;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IPublisherRepository _publisherRepository;
    private readonly BookService _bookService;

    public BooksController(
        IBookRepository bookRepository,
        IAuthorRepository authorRepository,
        ICategoryRepository categoryRepository,
        IPublisherRepository publisherRepository,
        BookService bookService
    )
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _categoryRepository = categoryRepository;
        _publisherRepository = publisherRepository;
        _bookService = bookService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllBooks()
    {
        var books = await _bookRepository.GetAllBooks();

        if (books == null) { throw new KeyNotFoundException("Books"); }

        var booksDto = await _bookService.MapBooks(books);

        var response = new ApiResponse
        {
            Result = booksDto,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Error = null
        };
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBook([FromRoute] string id)
    {
        var book = await _bookRepository.GetBookById(id);

        if (book == null) { throw new KeyNotFoundException("Book"); }

        // Parse authors Ids to strings
        List<string> authorsStringIds = book.AuthorIds.ConvertAll(id => id.ToString());
        var authors = await _authorRepository.GetAuthorsByIds(authorsStringIds);

        // Parse categories Ids to strings
        List<string> categoriesStringIds = book.CategoryIds.ConvertAll(id => id.ToString());
        var categories = await _categoryRepository.GetCategoriesByIds(categoriesStringIds);

        // Parse publishers
        List<string> publishersStringIds = book.PublisherIds.ConvertAll(id => id.ToString());
        var publishers = await _publisherRepository.GetPublishersbyIds(publishersStringIds);

        // Response Dto
        GetBookDto bookDto = new GetBookDto
        {
            Id = book.Id,
            Title = book.Title,
            Description = book.Description,
            Authors = authors ?? new List<string>(),
            Categories = categories ?? new List<string>(),
            Publishers = publishers ?? new List<string>()
        };

        var response = new ApiResponse
        {
            Result = bookDto,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Error = null
        };
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("author/{authorId}")]
    public async Task<IActionResult> GetBooksByAuthor([FromRoute] string authorId)
    {
        // Check if author exists
        var authorExists = await _authorRepository.AuthorExists(authorId);
        if (!authorExists) { throw new KeyNotFoundException("Author"); }

        // Check if book exists
        var books = await _bookRepository.GetBooksByAuthor(authorId);
        if (books == null) { throw new KeyNotFoundException("Books"); }

        // Check if there are no books
        if (books.Count() == 0)
        {
            return Ok(
                new ApiResponse
                {
                    Result = null,
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    Error = null
                }
            );
        }

        // Map response
        var booksDto = await _bookService.MapBooks(books);

        var response = new ApiResponse
        {
            Result = booksDto,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Error = null
        };
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("category/{categoryId}")]
    public async Task<IActionResult> GetBooksByCategory([FromRoute] string categoryId)
    {
        // Check if author exists
        var categoryExists = await _categoryRepository.CategoryExists(categoryId);
        if (!categoryExists) { throw new KeyNotFoundException("Category"); }

        // Check if book exists
        var books = await _bookRepository.GetBooksByCategory(categoryId);
        if (books == null) { throw new KeyNotFoundException("Category"); }

        // Check if there are no books
        if (books.Count() == 0)
        {
            return Ok(
                new ApiResponse
                {
                    Result = null,
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    Error = null
                }
            );
        }

        // Map response
        var booksDto = await _bookService.MapBooks(books);

        var response = new ApiResponse
        {
            Result = booksDto,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Error = null
        };
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("publisher/{publisherId}")]
    public async Task<IActionResult> GetBooksByPublisher([FromRoute] string publisherId)
    {
        // Check if author exists
        var publisherExists = await _publisherRepository.PublisherExists(publisherId);
        if (!publisherExists) { throw new KeyNotFoundException("Publisher"); }

        // Check if book exists
        var books = await _bookRepository.GetBooksByPublisher(publisherId);
        if (books == null) { throw new KeyNotFoundException("Book"); }

        // Check if there are no books
        if (books.Count() == 0)
        {
            return Ok(
                new ApiResponse
                {
                    Result = null,
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    Error = null
                }
            );
        }

        // Map response
        var booksDto = await _bookService.MapBooks(books);

        var response = new ApiResponse
        {
            Result = booksDto,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Error = null
        };
        return Ok(response);
    }

    [AllowAnonymous]
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

        await _bookRepository.AddAsync(newBook);

        var response = new ApiResponse
        {
            Result = newBook,
            IsSuccess = true,
            StatusCode = StatusCodes.Status201Created,
            Error = null
        };
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveBook(string bookId)
    {
        bool bookExists = await _bookRepository.BookExists(bookId);
        if (!bookExists) { throw new KeyNotFoundException("Book"); }

        bool deleteResult = await _bookRepository.RemoveAsync(bookId);
        if (!deleteResult) { throw new BadHttpRequestException("Could not delete book"); }

        var response = new ApiResponse
        {
            Result = null,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Error = null
        };
        return Ok(response);
    }


    // Book - Author
    [AllowAnonymous]
    [HttpPut("{bookId}/author/{authorId}")]
    public async Task<IActionResult> AddRemoveAuthorToBook([FromRoute] string bookId, [FromRoute] string authorId)
    {
        var bookExists = await _bookRepository.BookExists(bookId);
        if (!bookExists) { throw new KeyNotFoundException("Book"); }

        var authorExists = await _authorRepository.AuthorExists(authorId);
        if (!authorExists) { throw new KeyNotFoundException("Author"); }

        var authorAddedToBook = await _bookRepository.AddRemoveAuthorToBook(bookId, authorId);
        if (!authorAddedToBook) { throw new BadHttpRequestException("Could not update the book"); }

        var response = new ApiResponse
        {
            Result = null,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Error = null
        };
        return Ok(response);
    }

    // Book - Category
    [AllowAnonymous]
    [HttpPut("{bookId}/category/{categoryId}")]
    public async Task<IActionResult> AddRemoveCategoryToBook([FromRoute] string bookId, [FromRoute] string categoryId)
    {
        var bookExists = await _bookRepository.BookExists(bookId);
        if (!bookExists) { throw new KeyNotFoundException("Book"); }

        var categoryExists = await _categoryRepository.CategoryExists(categoryId);
        if (!categoryExists) { throw new KeyNotFoundException("Category"); }

        var categoryAddedToBook = await _bookRepository.AddRemoveCategoryToBook(bookId, categoryId);
        if (!categoryAddedToBook) { throw new BadHttpRequestException("Could not update the book"); }

        var response = new ApiResponse
        {
            Result = null,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Error = null
        };
        return Ok(response);
    }

    // Book - Publisher
    [AllowAnonymous]
    [HttpPut("{bookId}/publisher/{publisherId}")]
    public async Task<IActionResult> AddPublisherToBook([FromRoute] string bookId, [FromRoute] string publisherId)
    {
        var bookExists = await _bookRepository.BookExists(bookId);
        if (!bookExists) { throw new KeyNotFoundException("Book"); }

        var publisherExists = await _publisherRepository.PublisherExists(publisherId);
        if (!publisherExists) { throw new KeyNotFoundException("Publisher"); }

        var publisherAddedToBook = await _bookRepository.AddRemovePublisherToBook(bookId, publisherId);
        if (!publisherAddedToBook) { throw new BadHttpRequestException("Could not update the book"); }

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