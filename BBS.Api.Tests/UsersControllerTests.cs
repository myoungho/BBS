using BBS.Api.Controllers;
using BBS.Application.Services;
using BBS.Domain.Entities;
using BBS.Domain.Enums;
using BBS.Domain.Repositories;
using BBS.Infrastructure.Data;
using BBS.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BBS.Api.Tests;

public class UsersControllerTests
{
    private static (BbsContext context, UsersController controller) CreateController()
    {
        var options = new DbContextOptionsBuilder<BbsContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new BbsContext(options);
        IRepository<User> repository = new Repository<User>(context);
        IUserService service = new UserService(repository);
        var controller = new UsersController(service);
        return (context, controller);
    }

    [Fact]
    public async Task GetUsers_ReturnsAllUsers()
    {
        var (context, controller) = CreateController();
        using (context)
        {
            context.Users.Add(new User { Id = "a@example.com", Nickname = "a", PasswordHash = "p" });
            context.Users.Add(new User { Id = "b@example.com", Nickname = "b", PasswordHash = "p" });
            await context.SaveChangesAsync();

            var result = await controller.GetUsers();
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var users = Assert.IsType<List<User>>(ok.Value);
            Assert.Equal(2, users.Count);
        }
    }

    [Fact]
    public async Task GetUser_ReturnsUser_WhenExists()
    {
        var (context, controller) = CreateController();
        using (context)
        {
            context.Users.Add(new User { Id = "a@example.com", Nickname = "a", PasswordHash = "p" });
            await context.SaveChangesAsync();

            var result = await controller.GetUser("a@example.com");
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var user = Assert.IsType<User>(ok.Value);
            Assert.Equal("a@example.com", user.Id);
            Assert.Contains(Role.Reader, user.Roles);
        }
    }

    [Fact]
    public async Task GetUser_ReturnsNotFound_WhenMissing()
    {
        var (context, controller) = CreateController();
        using (context)
        {
            var result = await controller.GetUser("missing@example.com");
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
