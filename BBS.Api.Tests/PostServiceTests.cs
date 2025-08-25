using BBS.Application.Services;
using BBS.Domain.Entities;
using BBS.Infrastructure.Data;
using BBS.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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
        var postRepo = new Repository<Post>(context);
        var commentRepo = new Repository<Comment>(context);
        var attachmentRepo = new Repository<Attachment>(context);
        var settingRepo = new Repository<BbsSetting>(context);
        context.BbsSettings.Add(new BbsSetting { AllowedFileExtensions = "txt" });
        context.SaveChanges();
        return new PostService(postRepo, commentRepo, attachmentRepo, settingRepo);
    }

    [Fact]
    public async Task CreatePostAsync_Throws_WhenTitleMissing()
    {
        var service = CreateService();
        var post = new Post { Title = "", Content = "content" };

        await Assert.ThrowsAsync<ArgumentException>(
            () => service.CreatePostAsync(post, 1));
    }

    [Fact]
    public async Task AddAttachmentAsync_AddsAttachments()
    {
        var service = CreateService();
        var post = await service.CreatePostAsync(new Post { Title = "t", Content = "c" }, 1);
        await service.AddAttachmentAsync(post.Id, new Attachment { FileName = "a1.txt", FileUrl = "http://example.com/a1.txt" });
        await service.AddAttachmentAsync(post.Id, new Attachment { FileName = "a2.txt", FileUrl = "http://example.com/a2.txt" });

        var attachments = await service.GetAttachmentsAsync(post.Id);
        Assert.Equal(2, attachments.Count());
    }
}
