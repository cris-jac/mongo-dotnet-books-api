using API.Configurations;
using API.DTO;
using API.Interfaces;
using API.Models;
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

    // private readonly IMongoCollection<Book> _bookRepository;
    public BooksController(
        // IMongoDatabase mongoDatabase,
        // IOptions<DatabaseSettings> dbSettings
        IBookRepository bookRepository,
        IAuthorRepository authorRepository,
        ICategoryRepository categoryRepository,
        IPublisherRepository publisherRepository
    )
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _categoryRepository = categoryRepository;
        _publisherRepository = publisherRepository;
        // _bookRepository = mongoDatabase.GetCollection<Book>(dbSettings.Value.BookCollectionName);    
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllBooks()
    {
        // var books = await _bookRepository.Find(_ => true).ToListAsync();
        var books = await _bookRepository.GetAllBooks();

        if (books == null) return NotFound("No books yet :/");

        //
        var booksDto = new List<GetBookDto>();

        foreach (var book in books)
        {
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
            booksDto.Add(new GetBookDto
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Authors = authors ?? new List<string>(),
                Categories = categories ?? new List<string>(),
                Publishers = publishers ?? new List<string>()
            });
        }

        return Ok(booksDto);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBook(string id)
    {
        // var book = await _bookRepository.Find(b => b.Id == id).FirstOrDefaultAsync();
        var book = await _bookRepository.GetBookById(id);

        if (book == null) return NotFound("There are no books with this Id");

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

        return Ok(bookDto);
    }

    [AllowAnonymous]
    [HttpGet("GetByAuthor")]
    public async Task<IActionResult> GetBooksByAuthor(string authorId)
    {
        // Check if author exists
        var authorExists = await _authorRepository.AuthorExists(authorId);
        if (!authorExists) return NotFound("Author with this Id does not exist");

        // Check if book exists
        var books = await _bookRepository.GetBooksByAuthor(authorId);
        if (books == null) return NotFound("Error: Not found");

        // Check if there are no books
        if (books.Count() == 0) return Ok("There are no books here");

        // Map response
        var booksDto = new List<GetBookDto>();

        foreach (var book in books)
        {
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
            booksDto.Add(new GetBookDto
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Authors = authors ?? new List<string>(),
                Categories = categories ?? new List<string>(),
                Publishers = publishers ?? new List<string>()
            });
        }

        return Ok(booksDto);
    }

    [AllowAnonymous]
    [HttpGet("GetByCategory")]
    public async Task<IActionResult> GetBooksByCategory(string categoryId)
    {
        // Check if author exists
        var categoryExists = await _categoryRepository.CategoryExists(categoryId);
        if (!categoryExists) return NotFound("Category with this Id does not exist");

        // Check if book exists
        var books = await _bookRepository.GetBooksByCategory(categoryId);
        if (books == null) return NotFound("Error: Not found");

        // Check if there are no books
        if (books.Count() == 0) return Ok("There are no books here");

        // Map response
        var booksDto = new List<GetBookDto>();

        foreach (var book in books)
        {
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
            booksDto.Add(new GetBookDto
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Authors = authors ?? new List<string>(),
                Categories = categories ?? new List<string>(),
                Publishers = publishers ?? new List<string>()
            });
        }

        return Ok(booksDto);
    }

    [AllowAnonymous]
    [HttpGet("GetByPublisher")]
    public async Task<IActionResult> GetBooksByPublisher(string publisherId)
    {
        // Check if author exists
        var publisherExists = await _publisherRepository.PublisherExists(publisherId);
        if (!publisherExists) return NotFound("Publisher with this Id does not exist");

        // Check if book exists
        var books = await _bookRepository.GetBooksByPublisher(publisherId);
        if (books == null) return NotFound("Error: Not found");

        // Check if there are no books
        if (books.Count() == 0) return Ok("There are no books here");

        // Map response
        var booksDto = new List<GetBookDto>();

        foreach (var book in books)
        {
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
            booksDto.Add(new GetBookDto
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Authors = authors ?? new List<string>(),
                Categories = categories ?? new List<string>(),
                Publishers = publishers ?? new List<string>()
            });
        }

        return Ok(booksDto);
    }

    // [HttpGet("GetByName")]
    // public async Task<IActionResult> GetBookByName(string bookName)
    // {
    //     string bookNameLower = bookName.ToLower();

    //     var book = await _bookRepository.Find(b => b.Title.ToLower() == bookNameLower).FirstOrDefaultAsync();

    //     return Ok(book);
    // }

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
        if (!deleteResult) return BadRequest("Not deleted");

        return Ok("Book deleted");
    }


    // Put

    // Book - Author
    [AllowAnonymous]
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

    [AllowAnonymous]
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

    // Book - Category
    [AllowAnonymous]
    [HttpPut("AddCategoryToBook")]
    public async Task<IActionResult> AddCategoryToBook(string bookId, string categoryId)
    {
        var bookExists = await _bookRepository.BookExists(bookId);
        if (!bookExists) return NotFound("There is no book with this Id");

        var categoryExists = await _categoryRepository.CategoryExists(categoryId); 
        if (!categoryExists) return NotFound("There is no category with this Id");
        
        var categoryAddedToBook = await _bookRepository.AddCategoryToBook(bookId, categoryId);
        if (!categoryAddedToBook) return BadRequest("Error adding category to book");

        return Ok("Book's categories updated");
    }

    [AllowAnonymous]
    [HttpPut("RemoveCategoryFromBook")]
    public async Task<IActionResult> RemoveCategoryFromBook(string bookId, string categoryId)
    {
        var bookExists = await _bookRepository.BookExists(bookId);
        if (!bookExists) return NotFound("There is no book with this Id");

        var categoryExists = await _categoryRepository.CategoryExists(categoryId);
        if (!categoryExists) return NotFound("There is no category with this Id");
        
        var categoryRemovedFromBook = await _bookRepository.RemoveCategoryFromBook(bookId, categoryId);
        if (!categoryRemovedFromBook) return BadRequest("Error removing category from book");

        return Ok("Book's categories updated");
    }


    // Book - Publisher
    [AllowAnonymous]
    [HttpPut("AddPublisherToBook")]
    public async Task<IActionResult> AddPublisherToBook(string bookId, string publisherId)
    {
        var bookExists = await _bookRepository.BookExists(bookId);
        if (!bookExists) return NotFound("There is no book with this Id");
 
        var publisherExists = await _publisherRepository.PublisherExists(publisherId);
        if (!publisherExists) return NotFound("There is no publisher with this Id");
        
        var publisherAddedToBook = await _bookRepository.AddPublisherToBook(bookId, publisherId);
        if (!publisherAddedToBook) return BadRequest("Error adding publisher to book");

        return Ok("Book's publishers updated");
    }

    [AllowAnonymous]
    [HttpPut("RemovePublisherFromBook")]
    public async Task<IActionResult> RemovePublisherFromBook(string bookId, string publisherId)
    {
        var bookExists = await _bookRepository.BookExists(bookId);
        if (!bookExists) return NotFound("There is no book with this Id");

        // var categoryExists = await _categoryRepository.CategoryExists(categoryId);
        var publisherExists = await _publisherRepository.PublisherExists(publisherId);
        if (!publisherExists) return NotFound("There is no publisher with this Id");
        
        var publisherRemovedFromBook = await _bookRepository.RemovePublisherFromBook(bookId, publisherId);
        if (!publisherRemovedFromBook) return BadRequest("Error removing publisher from book");

        return Ok("Book's publishers updated");
    }
}