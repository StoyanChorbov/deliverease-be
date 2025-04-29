using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
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
            return Ok(await userService.GetAsync(username));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> GetProfile()
    {
        try
        {
            var username = User.Identity?.Name;
            if (username == null)
                return Unauthorized();

            return Ok(await userService.GetAsync(username));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            return Ok(await userService.GetAllAsync());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
    {
        var result = await userService.LoginAsync(userLoginDto);
        if (result == null)
            return Unauthorized();

        var (token, refreshToken) = result;
        if (token == null || refreshToken == null)
            return Unauthorized();

        return Ok(new { accessToken = token, refreshToken });
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto user)
    {
        try
        {
            await userService.RegisterAsync(user);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
    {
        var result = await userService.RefreshAsync(refreshToken);
        if (result == null)
            return Unauthorized();

        var (token, newRefreshToken) = result;
        if (token == null || newRefreshToken == null)
            return Unauthorized();

        return Ok(new { accessToken = token, refreshToken = newRefreshToken });
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] UserDto userDto)
    {
        try
        {
            var updatedUser = await userService.UpdateAsync(userDto);

            if (updatedUser == null)
                return NotFound();

            return Ok(updatedUser);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{username}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Delete(string username)
    {
        try
        {
            await userService.DeleteAsync(username);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}