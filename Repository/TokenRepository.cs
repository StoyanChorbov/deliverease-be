using System.Collections;
using Microsoft.EntityFrameworkCore;
using Model;
using Repository.Context;

namespace Repository;

public class TokenRepository(DelivereaseDbContext context)
{
    private readonly DbSet<JwtToken> _tokens = context.Set<JwtToken>();

    public async Task<JwtToken?> GetByRefreshTokenAsync(string token)
    {
        return await _tokens.FirstOrDefaultAsync(t => t.RefreshToken == token);
    }

    public async Task AddAsync(JwtToken token)
    {
        await _tokens.AddAsync(token);
        await context.SaveChangesAsync();
    }

    public async Task<JwtToken> GetTokenByUserId(Guid userId)
    {
        var query = _tokens
            .AsNoTrackingWithIdentityResolution()
            .Where(t => t.UserId == userId);
        
        var token = await query.FirstOrDefaultAsync();
        if (token == null)
        {
            throw new ArgumentException("Token not found");
        }
        return token;
    }
    
    public async Task RemoveAsync(JwtToken token)
    {
        _tokens.Remove(token);
        await context.SaveChangesAsync();
    }
}