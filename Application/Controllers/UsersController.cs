using Microsoft.AspNetCore.Mvc;
using Model.DTO;
using Service;

namespace Application.Controllers;

[ApiController]
[Route("/users")]
public class UsersController(UserService userService) : ControllerBase
{
    [HttpGet("{username}")]
    public async Task<IActionResult> Get(string username)
    {
        try
        {
            return Ok(await userService.Get(username));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            return Ok(await userService.GetAll());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Post([FromBody] UserRegisterDto user)
    {
        try
        {
            return Created($"{user.Username}", await userService.Add(user));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{username}")]
    public async Task<IActionResult> Delete(string username)
    {
        try
        {
            await userService.Delete(username);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}