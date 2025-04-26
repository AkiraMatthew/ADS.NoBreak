using Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController: ControllerBase
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
        var credential = new NetworkCredential(signUpRequest.Email.ToLower(), signUpRequest.Password);

        return Ok();
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
        return Ok();
    }
}
