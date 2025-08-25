using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using BBS.Domain.Entities;
using BBS.Domain.Repositories;

namespace BBS.Application.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Role> _roleRepository;

    public UserService(IRepository<User> userRepository, IRepository<Role> roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public async Task<bool> RegisterAsync(string loginId, string password, string nickname, IEnumerable<string>? roles = null)
    {
        if ((await _userRepository.GetAllAsync()).Any(u => u.LoginId == loginId)) return false;
        if ((await _userRepository.GetAllAsync()).Any(u => u.Nickname == nickname)) return false;

        var user = new User
        {
            LoginId = loginId,
            Nickname = nickname,
            PasswordHash = Hash(password)
        };

        var roleNames = roles?.ToList() ?? new List<string> { "Reader" };
        var existingRoles = await _roleRepository.GetAllAsync();
        foreach (var roleName in roleNames)
        {
            var role = existingRoles.FirstOrDefault(r => r.Name == roleName);
            if (role == null)
            {
                role = await _roleRepository.AddAsync(new Role { Name = roleName });
                existingRoles.Add(role);
            }
            user.UserRoles.Add(new UserRole { Role = role });
        }

        await _userRepository.AddAsync(user);
        return true;
    }

    public async Task<User?> AuthenticateAsync(string loginId, string password)
    {
        var user = (await _userRepository.GetAllAsync()).FirstOrDefault(u => u.LoginId == loginId);
        if (user == null) return null;
        return user.PasswordHash == Hash(password) ? user : null;
    }

    public async Task<List<User>> GetUsersAsync()
    {
        return await _userRepository.GetAllAsync();
    }

    public async Task<User?> GetUserAsync(string loginId)
    {
        return (await _userRepository.GetAllAsync()).FirstOrDefault(u => u.LoginId == loginId);
    }

    public async Task DeleteUserAsync(string loginId)
    {
        var user = await GetUserAsync(loginId);
        if (user != null)
        {
            await _userRepository.DeleteAsync(user.Id);
        }
    }

    private static string Hash(string input)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(bytes);
    }
}

