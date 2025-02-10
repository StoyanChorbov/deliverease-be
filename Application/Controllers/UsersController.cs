using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model;
using Model.DTO;

namespace Application.Controllers;

[ApiController]
[Route("/users")]
public class UsersController : ControllerBase
{
    private static readonly User TestUser = new User("pesho", "1234", "pesho", "peshov", "pesho@gmail.com");

    private readonly ILogger<UsersController> _logger;

    public UsersController(ILogger<UsersController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(TestUser);
    }
    
    [HttpPost("register")]
    public IActionResult Post([FromBody] UserRegisterDto user)
    {
        return Ok(user);
    }
}