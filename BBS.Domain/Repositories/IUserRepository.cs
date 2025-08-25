using BBS.Domain.Entities;

namespace BBS.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByNicknameAsync(string nickname);
    Task<User> AddAsync(User user);
}

