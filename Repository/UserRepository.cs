using Microsoft.EntityFrameworkCore;
using Model;
using Repository.Context;

namespace Repository;

public class UserRepository(DelivereaseDbContext context)
{
    private readonly IQueryable<User> _users = context.Users;

    public async Task<ICollection<User>> GetAllAsync(bool useNavigationalProperties = true, bool isReadOnly = true)
    {
        if (isReadOnly)
        {
            if (useNavigationalProperties)
            {
                return await _users
                    .AsNoTrackingWithIdentityResolution()
                    .Include(u => u.JwtTokens)
                    .Include(u => u.SenderDeliveries)
                    .Include(u => u.RecipientDeliveries)
                    .Include(u => u.DelivererDeliveries)
                    .ToListAsync();
            }
            return await _users.AsNoTrackingWithIdentityResolution().ToListAsync();
        }
        if (useNavigationalProperties)
        {
            return await _users
                .Include(u => u.JwtTokens)
                .Include(u => u.SenderDeliveries)
                .Include(u => u.RecipientDeliveries)
                .Include(u => u.DelivererDeliveries)
                .ToListAsync();
        }
        return await _users.ToListAsync();
    }

    public async Task<List<User>> GetAllByUsernamesAsync(List<string> usernames, bool useNavigationalProperties = true, bool isReadOnly = true)
    {
        if (isReadOnly)
        {
            if (useNavigationalProperties)
            {
                return await _users
                    .AsNoTrackingWithIdentityResolution()
                    .Include(u => u.JwtTokens)
                    .Where(u => usernames.Contains(u.UserName!))
                    .ToListAsync();
            }
            return await _users.AsNoTrackingWithIdentityResolution().Where(u => usernames.Contains(u.UserName!)).ToListAsync();
        }
        if (useNavigationalProperties)
        {
            return await _users
                .Include(u => u.JwtTokens)
                .Where(u => usernames.Contains(u.UserName!))
                .ToListAsync();
        }
        return await _users.Where(u => usernames.Contains(u.UserName!)).ToListAsync();
    }

    public async Task<User?> GetByJwtTokenAsync(string token, bool useNavigationalProperties = true, bool isReadOnly = true)
    {
        if (isReadOnly)
        {
            if (useNavigationalProperties)
            {
                return await _users
                    .AsNoTrackingWithIdentityResolution()
                    .Include(u => u.JwtTokens)
                    .FirstOrDefaultAsync(u => u.JwtTokens.Any(t => t.Token == token));
            }
            return await _users.AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync(u => u.JwtTokens.Any(t => t.Token == token));
        }
        if (useNavigationalProperties)
        {
            return await _users
                .Include(u => u.JwtTokens)
                .FirstOrDefaultAsync(u => u.JwtTokens.Any(t => t.Token == token));
        }
        return await _users.FirstOrDefaultAsync(u => u.JwtTokens.Any(t => t.Token == token));
    }

    public async Task<User?> GetByRefreshTokenAsync(JwtToken token, bool useNavigationalProperties = true, bool isReadOnly = true)
    {
        if (isReadOnly)
        {
            if (useNavigationalProperties)
            {
                return await _users
                    .AsNoTrackingWithIdentityResolution()
                    .Include(u => u.JwtTokens)
                    .FirstOrDefaultAsync(u => u.JwtTokens.Contains(token));
            }
            return await _users.AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync(u => u.JwtTokens.Contains(token));
        }
        if (useNavigationalProperties)
        {
            return await _users
                .Include(u => u.JwtTokens)
                .FirstOrDefaultAsync(u => u.JwtTokens.Contains(token));
        }
        return await _users.FirstOrDefaultAsync(u => u.JwtTokens.Contains(token));
    }

    public async Task AddDeliveryRecipientsAsync(Delivery delivery, List<string> recipientUsernames)
    {
        var users = await _users
            .Where(u => recipientUsernames.Contains(u.UserName!))
            .ToListAsync();

        foreach (var user in users)
        {
            user.RecipientDeliveries.Add(delivery);
        }

        await context.SaveChangesAsync();
    }
}