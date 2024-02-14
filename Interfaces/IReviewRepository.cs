using API.Models;

namespace API.Interfaces;

public interface IReviewRepository
{
    Task AddReview(Review review);
    Task<bool> UpdateReview(string reviewId, Review review);
    Task<bool> DeleteReview(string reviewId);
    Task<Review> GetReview(string id);
    Task<IEnumerable<Review>> GetReviews();
    Task<IEnumerable<Review>> GetReviewsByBook(string bookId);
    Task<IEnumerable<Review>> GetReviewsByReader(string readerId);
    Task<bool> ReviewExists(string reviewId);
}