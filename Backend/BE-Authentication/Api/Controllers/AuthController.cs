using Domain.DTO;
using Microsoft.AspNetCore.Mvc;

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
        return Ok();
    }
}
