using BBS.Domain.Entities;
using BBS.Domain.Repositories;
using BBS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BBS.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly BbsContext _context;

    public UserRepository(BbsContext context)
    {
        _context = context;
    }

    public Task<User?> GetByEmailAsync(string email) => _context.Users.FindAsync(email).AsTask();

    public Task<User?> GetByNicknameAsync(string nickname) =>
        _context.Users.FirstOrDefaultAsync(u => u.Nickname == nickname);

    public async Task<User> AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
}

