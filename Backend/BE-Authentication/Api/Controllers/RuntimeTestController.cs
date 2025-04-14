using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RunTimeTestController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("API is running");
    }
}