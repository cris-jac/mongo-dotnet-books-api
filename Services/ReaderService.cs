using API.DTO;
using API.Interfaces;
using API.Models;

namespace API.Services;

public class ReaderService
{
    private readonly IBookRepository _bookRepository;
    private readonly INationalityRepository _nationalityRepository;

    public ReaderService(
        IBookRepository bookRepository,
        INationalityRepository nationalityRepository
    )
    {
        _bookRepository = bookRepository;
        _nationalityRepository = nationalityRepository;
    }

    public async Task<List<GetReaderDto>> MapReaders(IEnumerable<Reader> readers)
    {
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

        return readersDto;
    }
}