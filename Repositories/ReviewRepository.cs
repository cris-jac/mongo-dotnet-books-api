using API.Configurations;
using API.Interfaces;
using API.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace API.Repositories;

public class ReviewRepository : IReviewRepository
{
    private IMongoCollection<Review> _reviewCollection;

    public ReviewRepository(
        IMongoDatabase database,
        IOptions<DatabaseSettings> dbSettings
    )
    {
        _reviewCollection = database.GetCollection<Review>(dbSettings.Value.ReviewCollectionName);
    }

    public async Task AddReview(Review review)
    {
        await _reviewCollection.InsertOneAsync(review);
    }

    public async Task<bool> DeleteReview(string reviewId)
    {
        var deleted = await _reviewCollection.DeleteOneAsync(r => r.Id == reviewId);
        return deleted.DeletedCount > 0;
    }

    public async Task<Review> GetReview(string id)
    {
        return await _reviewCollection.Find(r => r.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Review>> GetReviews()
    {
        return await _reviewCollection.Find(_ => true).ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetReviewsByBook(string bookId)
    {
        ObjectId bookObjectId = ObjectId.Parse(bookId);
        var filter = Builders<Review>.Filter.Eq(r => r.BookId, bookObjectId);
        return await _reviewCollection.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetReviewsByReader(string readerId)
    {
        ObjectId readerObjectId = ObjectId.Parse(readerId);
        var filter = Builders<Review>.Filter.Eq(r => r.ReaderId, readerObjectId);
        return await _reviewCollection.Find(filter).ToListAsync();
    }

    public async Task<bool> ReviewExists(string reviewId)
    {
        return await _reviewCollection.Find(r => r.Id == reviewId).AnyAsync();
    }

    public async Task<bool> UpdateReview(string reviewId, Review review)
    {
        var updated = await _reviewCollection.ReplaceOneAsync(r => r.Id == reviewId, review);
        return updated.ModifiedCount > 0;
    }
}