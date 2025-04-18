using Microsoft.EntityFrameworkCore;
using Model;
using Repository.Context;

namespace Repository;

public class UserRepository(DelivereaseDbContext context)
{
    private readonly DbSet<User> _users = context.Set<User>();

    public async Task<User?> Get(Guid id)
    {
        return await _users.FindAsync(id);
    }

    public async Task<User?> Get(string username)
    {
        return await _users.FirstOrDefaultAsync(u => u.UserName == username);
    }

    public async Task<ICollection<User>> GetAll()
    {
        return await _users.ToListAsync();
    }

    public async Task<List<User>> GetAllByUsernames(List<string> usernames)
    {
        return await _users.Where(u => usernames.Contains(u.UserName)).ToListAsync();
    }

    public async Task<User?> GetByJwtToken(string token)
    {
        return await _users.FirstOrDefaultAsync(u => u.JwtTokens.Any(t => t.Token == token));
    }

    public async Task<User?> GetByRefreshToken(JwtToken token)
    {
        return await _users.FirstOrDefaultAsync(u => u.JwtTokens.Contains(token));
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