using BBS.Application.Services;
using BBS.Domain.Entities;
using BBS.Infrastructure.Data;
using BBS.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BBS.Api.Tests;

public class PostServiceTests
{
    private static PostService CreateService()
    {
        var options = new DbContextOptionsBuilder<BbsContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new BbsContext(options);
        var postRepo = new Repository<Post, int>(context);
        var commentRepo = new Repository<Comment, int>(context);
        return new PostService(postRepo, commentRepo);
    }

    [Fact]
    public async Task CreatePostAsync_Throws_WhenTitleMissing()
    {
        var service = CreateService();
        var post = new Post { Title = "", Content = "content" };

        await Assert.ThrowsAsync<ArgumentException>(
            () => service.CreatePostAsync(post, "user@example.com"));
    }
}
