using API.DTO;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IReaderRepository _readerRepository;
    private readonly IBookRepository _bookRepository;

    public ReviewsController(
        IReviewRepository reviewRepository,
        IReaderRepository readerRepository,
        IBookRepository bookRepository
    )
    {
        _reviewRepository = reviewRepository;
        _readerRepository = readerRepository;
        _bookRepository = bookRepository;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetReviews()
    {
        var reviews = await _reviewRepository.GetReviews();
        if (reviews == null) { throw new KeyNotFoundException("Reviews"); }

        List<GetReviewDto> reviewsDto = new List<GetReviewDto>();

        foreach (var review in reviews)
        {
            var reader = await _readerRepository.GetReaderById(review.ReaderId.ToString());

            var book = await _bookRepository.GetBookById(review.BookId.ToString());

            reviewsDto.Add(new GetReviewDto
            {
                Id = review.Id,
                Book = book?.Title ?? $"Oops... there should be: {review.BookId}",
                Reader = reader?.Name ?? $"Weird... it should appear: {review.ReaderId}",
                Title = review.Title,
                Description = review.Description,
                Rating = review.Rating
            });
        }

        var response = new ApiResponse
        {
            Result = reviewsDto,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Error = null
        };
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetReview(string id)
    {
        var review = await _reviewRepository.GetReview(id);
        if (review == null) { throw new KeyNotFoundException("Review"); }

        var reader = await _readerRepository.GetReaderById(review.ReaderId.ToString());

        var book = await _bookRepository.GetBookById(review.BookId.ToString());

        var reviewDto = new GetReviewDto
        {
            Id = review.Id,
            Book = book.Title,
            Reader = reader.Name,
            Title = review.Title,
            Description = review.Description,
            Rating = review.Rating
        };

        var response = new ApiResponse
        {
            Result = reviewDto,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Error = null
        };
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("book/{bookId}")]
    public async Task<IActionResult> GetReviewsByBook([FromRoute] string bookId)
    {
        var reviews = await _reviewRepository.GetReviewsByBook(bookId);
        if (reviews == null) { throw new KeyNotFoundException("Reviews"); }

        var reviewsDto = new List<GetReviewDto>();

        foreach (var review in reviews)
        {
            var reader = await _readerRepository.GetReaderById(review.ReaderId.ToString());

            var book = await _bookRepository.GetBookById(review.BookId.ToString());

            reviewsDto.Add(new GetReviewDto
            {
                Id = review.Id,
                Book = book.Title,
                Reader = reader.Name,
                Title = review.Title,
                Description = review.Description,
                Rating = review.Rating
            });
        }

        var response = new ApiResponse
        {
            Result = reviewsDto,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Error = null
        };
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("reader/{readerId}")]
    public async Task<IActionResult> GetReviewsByReader([FromRoute] string readerId)
    {
        var reviews = await _reviewRepository.GetReviewsByReader(readerId);
        if (reviews == null) { throw new KeyNotFoundException("Reviews"); }

        var reviewsDto = new List<GetReviewDto>();

        foreach (var review in reviews)
        {
            var reader = await _readerRepository.GetReaderById(review.ReaderId.ToString());

            var book = await _bookRepository.GetBookById(review.BookId.ToString());

            reviewsDto.Add(new GetReviewDto
            {
                Id = review.Id,
                Book = book.Title,
                Reader = reader.Name,
                Title = review.Title,
                Description = review.Description,
                Rating = review.Rating
            });
        }

        var response = new ApiResponse
        {
            Result = reviewsDto,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Error = null
        };
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> AddReview([FromQuery] AddReviewDto reviewDto)
    {
        Review review = new Review
        {
            BookId = ObjectId.Parse(reviewDto.BookId),
            Title = reviewDto.Title,
            Description = reviewDto.Description,
            Rating = reviewDto.Rating,
            ReaderId = ObjectId.Parse(reviewDto.ReaderId)
        };

        await _reviewRepository.AddReview(review);

        var response = new ApiResponse
        {
            Result = null,
            IsSuccess = true,
            StatusCode = StatusCodes.Status201Created,
            Error = null
        };
        return Ok(response);
    }


    [HttpDelete]
    public async Task<IActionResult> DeleteReview(string reviewId)
    {
        var reviewExists = await _reviewRepository.ReviewExists(reviewId);
        if (!reviewExists) { throw new KeyNotFoundException("Review"); }

        var deleted = await _reviewRepository.DeleteReview(reviewId);
        if (!deleted) { throw new BadHttpRequestException("Could not delete review"); }

        var response = new ApiResponse
        {
            Result = null,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Error = null
        };
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPut]
    public async Task<IActionResult> UpdateReview([FromQuery] string reviewId, [FromBody] UpdateReviewDto reviewDto)
    {
        var reviewExists = await _reviewRepository.ReviewExists(reviewId);
        if (!reviewExists) { throw new KeyNotFoundException("Review"); }

        var review = await _reviewRepository.GetReview(reviewId);

        Review updatedReview = new Review
        {
            Id = review.Id,
            Title = reviewDto.Title,
            Description = reviewDto.Description,
            Rating = reviewDto.Rating,
            ReaderId = review.ReaderId,
            BookId = review.BookId
        };

        var updateResult = await _reviewRepository.UpdateReview(reviewId, updatedReview);
        if (!updateResult) { throw new BadHttpRequestException("Could not update review"); }

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