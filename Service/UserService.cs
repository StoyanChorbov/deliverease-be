using Model;
using Repository;

namespace Service;

public class UserService(UserRepository userRepository)
{
    public async Task<User> Add(User user)
    {
        var existingUser = await userRepository.Get(user.Id);
        
        if (existingUser != null)
            throw new Exception("User already exists");
        
        return await userRepository.Add(user);
    }
    
    public async Task<User> Get(Guid id)
    {
        var user = await userRepository.Get(id);
        
        if (user == null)
            throw new Exception("User not found");
        
        return user;
    }
    
    public async Task<User> Update(User user)
    {
        var existingUser = await userRepository.Get(user.Id);
        
        if (existingUser == null)
            throw new Exception("User not found");
        
        return await userRepository.Update(user);
    }
    
    public async Task Delete(Guid id) =>
        await userRepository.Delete(id);
}