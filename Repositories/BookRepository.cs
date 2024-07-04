using API.Configurations;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace API.Repositories;

public class BookRepository : IBookRepository
{
    private IMongoCollection<Book> _bookCollection;
    public BookRepository(
        IMongoDatabase database,
        IOptions<DatabaseSettings> dbSettings
    )
    {
        _bookCollection = database.GetCollection<Book>(dbSettings.Value.BookCollectionName);
    }
    public async Task AddAsync(Book book)
    {
        await _bookCollection.InsertOneAsync(book);
    }

    public async Task<bool> BookExists(string id)
    {
        return await _bookCollection.Find(b => b.Id == id).AnyAsync();
    }

    public async Task<IEnumerable<Book>> GetAllBooks()
    {
        return await _bookCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Book> GetBookById(string id)
    {
        return await _bookCollection.Find(b => b.Id == id).FirstOrDefaultAsync();
    }

    //
    public async Task<IEnumerable<Book>> GetBooksByAuthor(string authorId)
    {
        ObjectId authorObjectId = ObjectId.Parse(authorId);
        var filter = Builders<Book>.Filter.AnyIn(b => b.AuthorIds, new List<ObjectId> { authorObjectId });
        return await _bookCollection.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetBooksByCategory(string categoryId)
    {
        ObjectId categoryObjectId = ObjectId.Parse(categoryId);
        var filter = Builders<Book>.Filter.AnyIn(b => b.CategoryIds, new List<ObjectId> { categoryObjectId });
        return await _bookCollection.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetBooksByPublisher(string publisherId)
    {
        ObjectId publisherObjectId = ObjectId.Parse(publisherId);
        var filter = Builders<Book>.Filter.AnyIn(b => b.PublisherIds, new List<ObjectId> { publisherObjectId });
        return await _bookCollection.Find(filter).ToListAsync();
    }

    public async Task<bool> RemoveAsync(string id)
    {
        var deleted = await _bookCollection.DeleteOneAsync(b => b.Id == id);
        return deleted.DeletedCount > 0;
    }

    public async Task UpdateAsync(string id, Book book)
    {
        await _bookCollection.ReplaceOneAsync(b => b.Id == id, book);
    }

    // Update Add
    public async Task<bool> AddRemoveAuthorToBook(string bookId, string authorId)
    {
        // var updateDefinition = Builders<Book>.Update.Set(b => b.AuthorIds, ObjectId.Parse(authorId));
        var bookFiltered = Builders<Book>.Filter.Eq(b => b.Id, bookId);

        var book = await _bookCollection.Find(bookFiltered).FirstOrDefaultAsync();

        UpdateDefinition<Book> bookUpdated;
        if (book.AuthorIds.Contains(ObjectId.Parse(authorId)))
        {
            // remove
            bookUpdated = Builders<Book>.Update.Pull(b => b.AuthorIds, ObjectId.Parse(authorId));
        }
        else
        {
            // add
            bookUpdated = Builders<Book>.Update.AddToSet(b => b.AuthorIds, ObjectId.Parse(authorId));
        }

        var updated = await _bookCollection.UpdateOneAsync(bookFiltered, bookUpdated);

        return updated.ModifiedCount > 0;
    }

    public async Task<bool> AddRemovePublisherToBook(string bookId, string publisherId)
    {
        var bookFiltered = Builders<Book>.Filter.Eq(b => b.Id, bookId);

        var book = await _bookCollection.Find(bookFiltered).FirstOrDefaultAsync();

        UpdateDefinition<Book> bookUpdated;
        if (book.PublisherIds.Contains(ObjectId.Parse(publisherId)))
        {
            // remove
            bookUpdated = Builders<Book>.Update.Pull(b => b.PublisherIds, ObjectId.Parse(publisherId));
        }
        else
        {
            // add
            bookUpdated = Builders<Book>.Update.AddToSet(b => b.PublisherIds, ObjectId.Parse(publisherId));
        }

        var updated = await _bookCollection.UpdateOneAsync(bookFiltered, bookUpdated);

        return updated.ModifiedCount > 0;
    }

    public async Task<bool> AddRemoveCategoryToBook(string bookId, string categoryId)
    {
        var bookFiltered = Builders<Book>.Filter.Eq(b => b.Id, bookId);

        var book = await _bookCollection.Find(bookFiltered).FirstOrDefaultAsync();

        UpdateDefinition<Book> bookUpdated;
        if (book.CategoryIds.Contains(ObjectId.Parse(categoryId)))
        {
            // remove
            bookUpdated = Builders<Book>.Update.Pull(b => b.CategoryIds, ObjectId.Parse(categoryId));
        }
        else
        {
            // add
            bookUpdated = Builders<Book>.Update.AddToSet(b => b.CategoryIds, ObjectId.Parse(categoryId));
        }

        var updated = await _bookCollection.UpdateOneAsync(bookFiltered, bookUpdated);

        return updated.ModifiedCount > 0;
    }


    // External purposes
    public async Task<List<string>> GetBooksByIds(List<string> booksIds)
    {
        var booksFilter = Builders<Book>.Filter.In(b => b.Id, booksIds);

        var books = await _bookCollection.Find(booksFilter).ToListAsync();

        return books.Select(b => b.Title).ToList();
    }
}