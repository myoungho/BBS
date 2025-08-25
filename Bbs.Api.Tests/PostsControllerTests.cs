using Bbs.Api.Controllers;
using Bbs.Api.Data;
using Bbs.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Bbs.Api.Tests;

public class PostsControllerTests
{
    private static BbsContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<BbsContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new BbsContext(options);
    }

    [Fact]
    public async Task GetPost_ReturnsNotFound_WhenPostMissing()
    {
        using var context = CreateContext();
        var controller = new PostsController(context);

        var result = await controller.GetPost(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreatePost_PersistsPost()
    {
        using var context = CreateContext();
        var controller = new PostsController(context);
        var newPost = new Post { Title = "Hello", Content = "World" };

        var actionResult = await controller.CreatePost(newPost);
        var created = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        var createdPost = Assert.IsType<Post>(created.Value);

        Assert.Equal("Hello", createdPost.Title);
        Assert.Single(context.Posts);
    }
}
