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
}