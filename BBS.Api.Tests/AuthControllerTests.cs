using BBS.Api.Controllers;
using BBS.Application.Services;
using BBS.Domain.Entities;
using BBS.Domain.Enums;
using BBS.Domain.Repositories;
using BBS.Infrastructure.Data;
using BBS.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace BBS.Api.Tests;

public class AuthControllerTests
{
    private static (BbsContext context, AuthController controller) CreateController()
    {
        var options = new DbContextOptionsBuilder<BbsContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new BbsContext(options);
        IRepository<User> repository = new Repository<User>(context);
        IUserService service = new UserService(repository);
        var settings = new Dictionary<string, string>
        {
            { "Jwt:Key", "k" },
            { "Jwt:Issuer", "i" },
            { "Jwt:Audience", "a" }
        };
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(settings!)
            .Build();
        var controller = new AuthController(service, configuration);
        return (context, controller);
    }

    [Fact]
    public async Task Register_ReturnsConflict_WhenEmailExists()
    {
        var (context, controller) = CreateController();
        using (context)
        {
            await controller.Register(new RegisterDto("test@example.com", "pass", "nick1"));
            var result = await controller.Register(new RegisterDto("test@example.com", "pass", "nick2"));
            Assert.IsType<ConflictResult>(result);
        }
    }

    [Fact]
    public async Task Register_ReturnsConflict_WhenNicknameExists()
    {
        var (context, controller) = CreateController();
        using (context)
        {
            await controller.Register(new RegisterDto("test1@example.com", "pass", "nick"));
            var result = await controller.Register(new RegisterDto("test2@example.com", "pass", "nick"));
            Assert.IsType<ConflictResult>(result);
        }
    }

    [Fact]
    public async Task Register_AssignsReaderRoleByDefault()
    {
        var (context, controller) = CreateController();
        using (context)
        {
            await controller.Register(new RegisterDto("test@example.com", "pass", "nick"));
            var user = await context.Users.FirstOrDefaultAsync(u => u.LoginId == "test@example.com");
            Assert.NotNull(user);
            Assert.Contains(Role.Reader, user!.Roles);
        }
    }
}

