using BBS.Domain.Entities;

namespace BBS.Application.Services;

public interface IUserService
{
    Task<bool> RegisterAsync(string email, string password, string nickname);
    Task<User?> AuthenticateAsync(string email, string password);
}

