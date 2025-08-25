using System.Collections.Generic;
using BBS.Domain.Entities;

namespace BBS.Application.Services;

public interface IUserService
{
    Task<bool> RegisterAsync(string loginId, string password, string nickname, IEnumerable<string>? roles = null);
    Task<User?> AuthenticateAsync(string loginId, string password);
    Task<List<User>> GetUsersAsync();
    Task<User?> GetUserAsync(string loginId);
    Task DeleteUserAsync(string loginId);
}

