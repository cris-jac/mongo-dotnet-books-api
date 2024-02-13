using API.DTO;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
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
    

    [HttpGet]
    public async Task<IActionResult> GetAllPublishers()
    {
        var publishers = await _publisherRepository.GetPublishers();

        if (publishers == null) return NotFound("There are no publishers yet");

        return Ok(publishers);
    }

    [HttpPost]
    public async Task<IActionResult> AddPublisher(AddPublisherDto publisherDto)
    {
        Publisher newPublisher = new Publisher
        {
            Name = publisherDto.PublisherName
        };

        await _publisherRepository.AddPublisher(newPublisher);

        return Ok("Publisher added");
    }

    [HttpDelete]
    public async Task<IActionResult> DeletePublisher(string id)
    {
        var publisherExists = await _publisherRepository.PublisherExists(id);

        if (!publisherExists)
        {
            return NotFound("Publisher with this Id does not exist");
        }

        var deleteResult = await _publisherRepository.RemovePublisher(id);

        if (!deleteResult) return BadRequest("Error while deleteing this publisher");

        return Ok("Publisher removed");
    }
}