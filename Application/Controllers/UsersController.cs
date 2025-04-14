using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.DTO;
using Model.DTO.User;
using Service;

namespace Application.Controllers;

[ApiController]
[Route("/users")]
public class UsersController(UserService userService) : ControllerBase
{
    [HttpGet("{username}")]
    [Authorize]
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

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
    {
        var token = await userService.Login(userLoginDto);
        
        if (token == null)
            return Unauthorized();
        
        return Ok(token);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto user)
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
    
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
    {
        var token = await userService.Refresh(refreshToken);
        
        if (token == null)
            return Unauthorized();
        
        return Ok(token);
    }

    public async Task<IActionResult> Update(UserDto userDto)
    {
        try
        {
            var updatedUser = await userService.Update(userDto);
            return Ok(updatedUser);
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