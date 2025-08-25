using BBS.Api.Controllers;
using BBS.Application.Services;
using BBS.Domain.Entities;
using BBS.Domain.Repositories;
using BBS.Infrastructure.Data;
using BBS.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Xunit;

namespace BBS.Api.Tests;

public class PostsControllerTests
{
    private static (BbsContext context, PostsController controller) CreateController()
    {
        var options = new DbContextOptionsBuilder<BbsContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new BbsContext(options);
        IPostRepository repository = new PostRepository(context);
        IPostService service = new PostService(repository);
        var controller = new PostsController(service)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, "user@example.com")
                    }))
                }
            }
        };
        return (context, controller);
    }

    [Fact]
    public async Task GetPost_ReturnsNotFound_WhenPostMissing()
    {
        var (context, controller) = CreateController();
        using (context)
        {
            var result = await controller.GetPost(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }
    }

    [Fact]
    public async Task CreatePost_PersistsPost()
    {
        var (context, controller) = CreateController();
        using (context)
        {
            var newPost = new Post { Title = "Hello", Content = "World" };

            var actionResult = await controller.CreatePost(newPost);
            var created = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var createdPost = Assert.IsType<Post>(created.Value);

            Assert.Equal("Hello", createdPost.Title);
            Assert.Equal("user@example.com", createdPost.AuthorId);
            Assert.Single(context.Posts);
        }
    }
}
