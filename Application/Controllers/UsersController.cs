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
    // Get user by username
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

    // Get the data for the logged-in user's profile
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

    // Get all users
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

    
    // Login with username and password
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

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var username = User.Identity?.Name;
        if (username == null)
            return Unauthorized();

        await userService.LogoutAsync(username);
        return Ok();
    }

    // Register a new user
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

    // Refresh the access token using the refresh token
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

    // Update the user's profile
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

    // Delete a user by username
    // Requires admin role
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