using Domain.DTO;
using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Services;
using System.Net;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IUserService userService): ControllerBase
{
    /// <summary>
    /// Registers a new user
    /// </summary>
    /// <param name="signUpRequest">Input data containing the new registered user data</param>
    /// <returns>
    /// Case of success: Token is stored in cookie;
    /// Case of failure: returns BadRequest
    /// </returns>
    [HttpPost]
    [Route("SignUp")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignUpAsync([FromBody] SignUpDTO signUpRequest)
    {
        try
        {
            var credentials = new NetworkCredential(signUpRequest.Email.ToLower(), signUpRequest.Password);
            var isTokenCreated = await userService.SignUpAsync(credentials, signUpRequest);

            if(string.IsNullOrEmpty(isTokenCreated))
                return BadRequest("User already exists!");

            return Ok();
        }
        catch (Exception ex) 
        {
            return BadRequest($"User could not be created, details: {ex.Message}");
        }
    }

    /// <summary>
    /// Registers a new user
    /// </summary>
    /// <param name="signInRequest">Input data containing the user data</param>
    /// <returns>
    /// Case of success: Token is stored in cookie;
    /// Case of failure: returns BadRequest
    /// </returns>
    [HttpPost]
    [Route("SignIn")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignInAsync([FromBody] SignInDTO signInRequest)
    {
        try
        {
            var credentials = new NetworkCredential(signInRequest.Email, signInRequest.Password);

            var token = await userService.SignInAsync(credentials);

            if (token is null)
                return BadRequest();

            return Ok(token);
        }
        catch (Exception ex) 
        {
            return BadRequest($"User authentication failed, details: {ex.Message}");
        }
    }
}
