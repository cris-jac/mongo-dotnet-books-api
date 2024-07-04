using API.DTO;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class PublishersController : ControllerBase
{
    private readonly IPublisherRepository _publisherRepository;

    public PublishersController(
        IPublisherRepository publisherRepository
    )
    {
        _publisherRepository = publisherRepository;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllPublishers()
    {
        var publishers = await _publisherRepository.GetPublishers();

        if (publishers == null) { throw new KeyNotFoundException("Publishers"); }

        var response = new ApiResponse
        {
            Result = publishers,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Error = null
        };
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> AddPublisher(AddPublisherDto publisherDto)
    {
        Publisher newPublisher = new Publisher
        {
            Name = publisherDto.PublisherName
        };

        await _publisherRepository.AddPublisher(newPublisher);

        var response = new ApiResponse
        {
            Result = null,
            IsSuccess = true,
            StatusCode = StatusCodes.Status201Created,
            Error = null
        };
        return Ok(response);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePublisher(string id)
    {
        var publisherExists = await _publisherRepository.PublisherExists(id);

        if (!publisherExists) { throw new KeyNotFoundException("Publisher"); }

        var deleteResult = await _publisherRepository.RemovePublisher(id);

        if (!deleteResult) { throw new BadHttpRequestException("Could not delete publisher"); }

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