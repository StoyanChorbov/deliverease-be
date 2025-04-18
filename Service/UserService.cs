using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Model;
using Model.DTO.User;
using Repository;

namespace Service;

public class UserService(
    UserRepository userRepository,
    TokenRepository tokenRepository,
    UserManager<User> userManager,
    IConfiguration configuration)
{
    public async Task<UserDto> Get(Guid id)
    {
        var user = await userRepository.Get(id);

        if (user == null)
            throw new Exception("User not found");
        return ToDto(user);
    }

    public async Task<UserDto> Get(string username)
    {
        return ToDto(await GetUserByUsername(username));
    }

    public async Task<User> GetUserByUsername(string username)
    {
        var user = await userRepository.Get(username);

        if (user == null)
            throw new Exception("User not found");

        return user;
    }

    public async Task<List<User>> GetAllByUsernames(List<string> usernames)
    {
        return await userRepository.GetAllByUsernames(usernames);
    }

    public async Task<User?> GetByJwtToken(string token)
    {
        return await userRepository.GetByJwtToken(token);
    }

    public async Task<Tuple<string?, string?>?> Login(UserLoginDto userLoginDto)
    {
        var username = userManager.NormalizeName(userLoginDto.Username);
        var password = userLoginDto.Password;
        var user = await userManager.FindByNameAsync(username);

        if (user == null || !await userManager.CheckPasswordAsync(user, password)) return null;

        var generatedAuthToken = await GenerateAuthToken(user);
        var authToken = new JwtSecurityTokenHandler().WriteToken(generatedAuthToken);
        var refreshToken = GenerateRefreshToken();

        var token = new JwtToken
        {
            Token = authToken,
            RefreshToken = refreshToken.Token,
            RefreshTokenExpiry = refreshToken.Expires
        };

        await tokenRepository.AddAsync(token);
        user.JwtTokens.Add(token);

        await userManager.UpdateAsync(user);

        return new Tuple<string?, string?>(
            authToken,
            refreshToken.Token
        );
    }

    public async Task<Tuple<string?, string?>?> Refresh(string refreshToken)
    {
        var existingToken = await tokenRepository.GetByRefreshTokenAsync(refreshToken);

        if (existingToken == null || existingToken.RefreshTokenExpiry < DateTime.UtcNow)
            throw new UnauthorizedAccessException("Invalid or expired refresh token!");

        var user = await userRepository.GetByRefreshToken(existingToken);

        if (user == null)
            throw new ArgumentException("User not found");

        var generatedAuthToken = await GenerateAuthToken(user);
        var authToken = new JwtSecurityTokenHandler().WriteToken(generatedAuthToken);
        var generatedRefreshToken = GenerateRefreshToken();

        var token = new JwtToken
        {
            Token = authToken,
            RefreshToken = generatedRefreshToken.Token,
            RefreshTokenExpiry = generatedRefreshToken.Expires
        };

        await tokenRepository.AddAsync(token);
        user.JwtTokens.Add(token);

        await userManager.UpdateAsync(user);

        return new Tuple<string?, string?>(
            authToken,
            refreshToken
        );
    }

    public async Task<ICollection<UserDto>> GetAll()
    {
        return (await userRepository.GetAll()).Select(ToDto).ToList();
    }

    public async Task Register(UserRegisterDto user)
    {
        var existingUser = await userRepository.Get(user.Username);

        if (existingUser != null)
            throw new Exception("User already exists");

        var newUser = new User
        {
            UserName = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };

        var result = await userManager.CreateAsync(newUser, user.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new Exception($"Failed to create user: {errors}");
        }

        await userManager.AddToRoleAsync(newUser, UserRoles.User);
    }

    public async Task<UserDto?> Update(UserDto user)
    {
        var existingUser = await userRepository.Get(user.Username);

        if (existingUser == null)
            throw new Exception("User not found");

        var updateResult = await userManager.UpdateAsync(existingUser);

        return updateResult.Succeeded ? ToDto(existingUser) : null;
    }

    public async Task Delete(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());

        if (user == null)
            throw new Exception("User not found");

        await userManager.DeleteAsync(user);
    }

    public async Task Delete(string username)
    {
        var normalizedUserName = userManager.NormalizeName(username);
        var user = await userManager.FindByNameAsync(normalizedUserName);

        if (user == null)
            throw new Exception("User not found");

        await userManager.DeleteAsync(user);
    }

    public static string? CheckTokenValidity(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        if (!tokenHandler.CanReadToken(token))
            return "Invalid token";

        var jwtToken = tokenHandler.ReadJwtToken(token);
        var expiryClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);

        if (expiryClaim == null)
            return "Token does not contain expiry claim";

        var expiryDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expiryClaim.Value)).UtcDateTime;
        if (expiryDate < DateTime.UtcNow)
            return "Expired token";

        return null;
    }

    private static UserDto ToDto(User user)
    {
        return new UserDto(user.UserName, user.FirstName, user.LastName, user.Email, user.PhoneNumber,
            user.IsDeliveryPerson);
    }

    private JwtSecurityToken GetToken(List<Claim> claims)
    {
        var secretKey = configuration["JWT:SecretKey"] ?? throw new Exception("Secret key not found");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
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

        var roles = await userManager.GetRolesAsync(user);
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