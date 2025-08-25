using System.Collections.Generic;
using BBS.Domain.Entities;

namespace BBS.Application.Services;

public interface IUserService
{
    Task<bool> RegisterAsync(string email, string password, string nickname);
    Task<User?> AuthenticateAsync(string email, string password);
    Task<List<User>> GetUsersAsync();
    Task<User?> GetUserAsync(string email);
    Task DeleteUserAsync(string email);
}

