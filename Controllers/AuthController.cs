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
    public AuthController(
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
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userRepository.GetUsers();

        if (users == null) { throw new KeyNotFoundException("Users"); }

        var response = new ApiResponse
        {
            Result = users,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Error = null
        };
        return Ok(response);
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        // Get user by email
        var user = await _userRepository.GetUserByEmail(registerDto.Email);

        // Check email
        if (user != null) { throw new BadHttpRequestException("The email is already taken"); }

        // Register Service
        var newUser = _registerService.Register(registerDto);

        // Add
        await _userRepository.AddUserAsync(newUser);

        var response = new ApiResponse
        {
            Result = null,
            IsSuccess = true,
            StatusCode = StatusCodes.Status201Created,
            Error = null
        };
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto requestDto)
    {
        // Get by email
        var user = await _userRepository.GetUserByEmail(requestDto.Email);

        // Check if exists
        if (user == null) { throw new KeyNotFoundException("This email is not registered"); }

        // Check Password
        if (!_loginService.Login(user, requestDto.Password)) { throw new BadHttpRequestException("Invalid credentials"); }

        // Map
        var loginResponse = new LoginResponseDto
        {
            Name = user.Name,
            Username = user.Username,
            JwtToken = _tokenService.GenerateToken(user)
        };

        var response = new ApiResponse
        {
            Result = loginResponse,
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Error = null
        };
        return Ok(response);
    }
}