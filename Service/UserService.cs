using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Model;
using Model.DTO;
using Model.DTO.User;
using Repository;

namespace Service;

public class UserService
{
    private readonly UserRepository _userRepository;
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;

    public UserService(UserRepository userRepository, UserManager<User> userManager, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<UserDto> Get(Guid id)
    {
        var user = await _userRepository.Get(id);

        if (user == null)
            throw new Exception("User not found");
        return ToDto(user);
    }

    public async Task<UserDto> Get(string username)
    {
        var user = await _userRepository.Get(username);

        if (user == null)
            throw new Exception("User not found");

        return ToDto(user);
    }

    public async Task<Tuple<string?, string?>?> Login(UserLoginDto userLoginDto)
    {
        var username = userLoginDto.Username;
        var password = userLoginDto.Password;
        var user = await _userManager.FindByNameAsync(username);

        if (user == null || !await _userManager.CheckPasswordAsync(user, password))
        {
            return null;
        }

        var authToken = await GenerateAuthToken(user);
        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken.Token;
        user.RefreshTokenExpiry = refreshToken.Expires;

        await _userManager.UpdateAsync(user);

        return new Tuple<string?, string?>(
            new JwtSecurityTokenHandler().WriteToken(authToken),
            refreshToken.Token
        );
    }

    public async Task<Tuple<string?, string?>?> Refresh(string refreshToken)
    {
        var user = await _userRepository.GetByRefreshToken(refreshToken);

        if (user == null || user.RefreshTokenExpiry < DateTime.Now)
        {
            throw new UnauthorizedAccessException("Invalid or expired refresh token!");
        }

        var authToken = await GenerateAuthToken(user);
        var newRefreshToken = GenerateRefreshToken();

        user.RefreshToken = newRefreshToken.Token;
        user.RefreshTokenExpiry = newRefreshToken.Expires;

        await _userManager.UpdateAsync(user);

        return new Tuple<string?, string?>(
            new JwtSecurityTokenHandler().WriteToken(authToken),
            refreshToken
        );
    }

    public async Task<ICollection<UserDto>> GetAll() =>
        (await _userRepository.GetAll()).Select(ToDto).ToList();

    public async Task<UserDto> Add(UserRegisterDto user)
    {
        var existingUser = await _userRepository.Get(user.Username);

        if (existingUser != null)
            throw new Exception("User already exists");

        return ToDto(await _userRepository.Add(new User
        {
            UserName = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        }));
    }

    public async Task<UserDto> Update(UserDto user)
    {
        var existingUser = await _userRepository.Get(user.Username);

        if (existingUser == null)
            throw new Exception("User not found");

        return ToDto(await _userRepository.Update(existingUser));
    }

    public async Task Delete(Guid id) =>
        await _userRepository.Delete(id);

    public async Task Delete(string username) =>
        await _userRepository.Delete(username);

    private static UserDto ToDto(User user) =>
        new(user.UserName, user.FirstName, user.LastName, user.Email, user.PhoneNumber, user.IsDeliveryPerson);

    private static JwtSecurityToken GetToken(List<Claim> claims)
    {
        var key = new SymmetricSecurityKey("JWT:SecretKey"u8.ToArray());
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            expires: DateTime.Now.AddHours(1),
            claims: claims,
            signingCredentials: credentials
        );
        return token;
    }

    private async Task<JwtSecurityToken> GenerateAuthToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName ?? throw new Exception("User has no username")),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return GetToken(claims);
    }

    private static RefreshToken GenerateRefreshToken()
    {
        return new RefreshToken
        {
            Token = Guid.NewGuid().ToString(),
            Expires = DateTime.UtcNow.AddDays(7)
        };
    }
}