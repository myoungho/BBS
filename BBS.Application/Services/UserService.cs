using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using BBS.Domain.Entities;
using BBS.Domain.Enums;
using BBS.Domain.Repositories;

namespace BBS.Application.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _repository;

    public UserService(IRepository<User> repository)
    {
        _repository = repository;
    }

    public async Task<bool> RegisterAsync(string loginId, string password, string nickname, IEnumerable<Role>? roles = null)
    {
        if ((await _repository.GetAllAsync()).Any(u => u.LoginId == loginId)) return false;
        if ((await _repository.GetAllAsync()).Any(u => u.Nickname == nickname)) return false;

        var user = new User
        {
            LoginId = loginId,
            Nickname = nickname,
            PasswordHash = Hash(password),
            Roles = roles?.ToList() ?? new List<Role> { Role.Reader }
        };
        await _repository.AddAsync(user);
        return true;
    }

    public async Task<User?> AuthenticateAsync(string loginId, string password)
    {
        var user = (await _repository.GetAllAsync()).FirstOrDefault(u => u.LoginId == loginId);
        if (user == null) return null;
        return user.PasswordHash == Hash(password) ? user : null;
    }

    public async Task<List<User>> GetUsersAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<User?> GetUserAsync(string loginId)
    {
        return (await _repository.GetAllAsync()).FirstOrDefault(u => u.LoginId == loginId);
    }

    public async Task DeleteUserAsync(string loginId)
    {
        var user = await GetUserAsync(loginId);
        if (user != null)
        {
            await _repository.DeleteAsync(user.Id);
        }
    }

    private static string Hash(string input)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(bytes);
    }
}

