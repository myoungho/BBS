using System.Security.Cryptography;
using System.Text;
using BBS.Domain.Entities;
using BBS.Domain.Repositories;

namespace BBS.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> RegisterAsync(string email, string password, string nickname)
    {
        if (await _repository.GetByEmailAsync(email) != null) return false;
        if (await _repository.GetByNicknameAsync(nickname) != null) return false;

        var user = new User
        {
            Id = email,
            Nickname = nickname,
            PasswordHash = Hash(password)
        };
        await _repository.AddAsync(user);
        return true;
    }

    public async Task<User?> AuthenticateAsync(string email, string password)
    {
        var user = await _repository.GetByEmailAsync(email);
        if (user == null) return null;
        return user.PasswordHash == Hash(password) ? user : null;
    }

    private static string Hash(string input)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(bytes);
    }
}

