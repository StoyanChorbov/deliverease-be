using Microsoft.EntityFrameworkCore;
using Model;
using Repository.Context;

namespace Repository;

public class UserRepository(DelivereaseDbContext context)
{
    private readonly DbSet<User> _users = context.Set<User>();
    
    public async Task<User?> Get(Guid id) =>
        await _users.FindAsync(id);
    
    public async Task<User?> Get(string username) =>
        await _users.FirstOrDefaultAsync(u => u.UserName == username);
    
    public async Task<ICollection<User>> GetAll() =>
        await _users.ToListAsync();
    
    public async Task<User?> GetByRefreshToken(string refreshToken) =>
        await _users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
    
    public async Task<User> Add(User user)
    {
        var result = await _users.AddAsync(user);
        await context.SaveChangesAsync();
        return result.Entity;
    }
    
    public async Task<User> Update(User user)
    {
        var result = _users.Update(user);
        await context.SaveChangesAsync();
        return result.Entity;
    }
    
    public async Task Delete(Guid id)
    {
        var user = await Get(id);
        
        if (user == null)
            throw new Exception("User not found");
        
        _users.Remove(user);
        await context.SaveChangesAsync();
    }
    
    public async Task Delete(string username)
    {
        var user = await Get(username);
        
        if (user == null)
            throw new Exception("User not found");
        
        _users.Remove(user);
        await context.SaveChangesAsync();
    }
}