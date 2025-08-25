using System.Collections.Generic;
using BBS.Domain.Entities;
using BBS.Domain.Enums;

namespace BBS.Application.Services;

public interface IUserService
{
    Task<bool> RegisterAsync(string loginId, string password, string nickname, IEnumerable<Role>? roles = null);
    Task<User?> AuthenticateAsync(string loginId, string password);
    Task<List<User>> GetUsersAsync();
    Task<User?> GetUserAsync(string loginId);
    Task DeleteUserAsync(string loginId);
}

