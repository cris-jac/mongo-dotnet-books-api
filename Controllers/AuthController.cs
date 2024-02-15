using API.Configurations;
using API.DTO;
using API.Interfaces;
using API.Models;
using API.Repositories;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace API.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly RegisterService _registerService;
    private readonly LoginService _loginService;
    private readonly TokenService _tokenService;
    private readonly IUserRepository _userRepository;
    // private readonly IOptions<DatabaseSettings> _dbSettings;
    // private readonly IMongoCollection<User> _userCollection;
    public AuthController(
        // IOptions<DatabaseSettings> dbSettings,
        IUserRepository userRepository,
        RegisterService registerService,
        LoginService loginService,
        TokenService tokenService
        )
    {
        _userRepository = userRepository;
        _registerService = registerService;
        _loginService = loginService;
        _tokenService = tokenService;
        // _dbSettings = dbSettings;
        // var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);
        // var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);
        // _userCollection = mongoDatabase.GetCollection<User>(dbSettings.Value.UserCollectionName);
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userRepository.GetUsers();

        if (users == null) return NotFound("No users");

        return Ok(users);
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        // Get user by email
        // var user = await _userCollection.Find(u => u.Email == registerDto.Email).FirstOrDefaultAsync();
        var user = await _userRepository.GetUserByEmail(registerDto.Email);

        // Check email
        if (user != null) 
        {
            return BadRequest("The email is already taken");
        }

        // Register Service
        var newUser = _registerService.Register(registerDto);
        
        // Add
        // await _userCollection.InsertOneAsync(newUser);
        await _userRepository.AddUserAsync(newUser);

        // 
        return Ok("User registered");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto requestDto)
    {
        // Get by email
        var user = await _userRepository.GetUserByEmail(requestDto.Email);

        // Check if exists
        if (user == null)
        {
            return NotFound("This email is not registered! Please register");
        }

        // Check Password
        if (!_loginService.Login(user, requestDto.Password))
        {
            return BadRequest("Invalid password");
        }

        // Map
        var loginResponse = new LoginResponseDto
        {
            Name = user.Name,
            Username = user.Username,
            JwtToken = _tokenService.GenerateToken(user)
        };

        return Ok(loginResponse);
    }
}