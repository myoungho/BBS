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

    public async Task<bool> RegisterAsync(string email, string password, string nickname, IEnumerable<Role>? roles = null)
    {
        if (await _repository.GetByIdAsync(email) != null) return false;
        if ((await _repository.GetAllAsync()).Any(u => u.Nickname == nickname)) return false;

        var user = new User
        {
            Id = email,
            Nickname = nickname,
            PasswordHash = Hash(password),
            Roles = roles?.ToList() ?? new List<Role> { Role.Reader }
        };
        await _repository.AddAsync(user);
        return true;
    }

    public async Task<User?> AuthenticateAsync(string email, string password)
    {
        var user = await _repository.GetByIdAsync(email);
        if (user == null) return null;
        return user.PasswordHash == Hash(password) ? user : null;
    }

    public async Task<List<User>> GetUsersAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<User?> GetUserAsync(string email)
    {
        return await _repository.GetByIdAsync(email);
    }

    public async Task DeleteUserAsync(string email)
    {
        await _repository.DeleteAsync(email);
    }

    private static string Hash(string input)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(bytes);
    }
}

