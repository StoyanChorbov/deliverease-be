using Model;
using Model.DTO;
using Repository;

namespace Service;

public class UserService(UserRepository userRepository)
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
        var user = await userRepository.Get(username);
        
        if (user == null)
            throw new Exception("User not found");
        
        return ToDto(user);
    }
    
    public async Task<ICollection<UserDto>> GetAll() =>
        (await userRepository.GetAll()).Select(ToDto).ToList();
    
    public async Task<UserDto> Add(UserRegisterDto user)
    {
        var existingUser = await userRepository.Get(user.Username);
        
        if (existingUser != null)
            throw new Exception("User already exists");
        
        return ToDto(await userRepository.Add(new User
        {
            Username = user.Username,
            Password = user.Password,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        }));
    }
    
    public async Task<UserDto> Update(UserDto user)
    {
        var existingUser = await userRepository.Get(user.Username);
        
        if (existingUser == null)
            throw new Exception("User not found");
        
        return ToDto(await userRepository.Update(existingUser));
    }
    
    public async Task Delete(Guid id) =>
        await userRepository.Delete(id);
    
    public async Task Delete(string username) =>
        await userRepository.Delete(username);
    
    private static UserDto ToDto(User user) =>
        new UserDto(user.Username, user.FirstName, user.LastName, user.Email, user.PhoneNumber, user.IsDeliveryPerson);
}