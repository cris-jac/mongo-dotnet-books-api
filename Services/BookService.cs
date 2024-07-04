using API.DTO;
using API.Interfaces;
using API.Models;

namespace API.Services;

public class BookService
{
    private IAuthorRepository _authorRepository;
    private ICategoryRepository _categoryRepository;
    private IPublisherRepository _publisherRepository;
    public BookService(
        IAuthorRepository authorRepository,
        ICategoryRepository categoryRepository,
        IPublisherRepository publisherRepository
    )
    {
        _authorRepository = authorRepository;
        _categoryRepository = categoryRepository;
        _publisherRepository = publisherRepository;
    }

    public async Task<List<GetBookDto>> MapBooks(IEnumerable<Book> books)
    {
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

        return booksDto;
    }
}