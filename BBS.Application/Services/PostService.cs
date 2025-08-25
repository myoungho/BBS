using System;
using System.Collections.Generic;
using System.Linq;
using BBS.Domain.Entities;
using BBS.Domain.Repositories;

namespace BBS.Application.Services;

public class PostService : IPostService
{
    private readonly IRepository<Post> _posts;
    private readonly IRepository<Comment> _comments;
    private readonly IRepository<Attachment> _attachments;

    public PostService(IRepository<Post> posts, IRepository<Comment> comments, IRepository<Attachment> attachments)
    {
        _posts = posts;
        _comments = comments;
        _attachments = attachments;
    }

    public async Task<IEnumerable<Post>> GetPostsAsync() => await _posts.GetAllAsync();

    public Task<Post?> GetPostAsync(int id) => _posts.GetByIdAsync(id);

    public Task<Post> CreatePostAsync(Post post, int authorId)
    {
        if (string.IsNullOrWhiteSpace(post.Title))
            throw new ArgumentException("Title is required", nameof(post));
        if (string.IsNullOrWhiteSpace(post.Content))
            throw new ArgumentException("Content is required", nameof(post));

        post.AuthorId = authorId;
        return _posts.AddAsync(post);
    }

    public async Task UpdatePostAsync(Post post, int userId)
    {
        if (string.IsNullOrWhiteSpace(post.Title) || string.IsNullOrWhiteSpace(post.Content))
            throw new ArgumentException("Title and content are required", nameof(post));

        var existing = await _posts.GetByIdAsync(post.Id);
        if (existing == null) throw new KeyNotFoundException("Post not found");
        if (existing.AuthorId != userId) throw new UnauthorizedAccessException("Cannot edit others' posts");
        existing.Title = post.Title;
        existing.Content = post.Content;
        await _posts.UpdateAsync(existing);
    }

    public async Task DeletePostAsync(int id, int userId)
    {
        var existing = await _posts.GetByIdAsync(id);
        if (existing == null) throw new KeyNotFoundException("Post not found");
        if (existing.AuthorId != userId) throw new UnauthorizedAccessException("Cannot delete others' posts");
        await _posts.DeleteAsync(id);
    }

    public async Task<Comment> AddCommentAsync(int postId, Comment comment)
    {
        if (string.IsNullOrWhiteSpace(comment.Content))
            throw new ArgumentException("Content is required", nameof(comment));

        var post = await _posts.GetByIdAsync(postId);
        if (post == null) throw new InvalidOperationException("Post not found");
        comment.PostId = postId;
        return await _comments.AddAsync(comment);
    }

    public async Task<IEnumerable<Comment>> GetCommentsAsync(int postId)
    {
        var post = await _posts.GetByIdAsync(postId);
        if (post == null) throw new InvalidOperationException("Post not found");
        var comments = await _comments.GetAllAsync();
        return comments.Where(c => c.PostId == postId);
    }

    public async Task<Attachment> AddAttachmentAsync(int postId, Attachment attachment)
    {
        if (string.IsNullOrWhiteSpace(attachment.FileName))
            throw new ArgumentException("File name is required", nameof(attachment));

        var post = await _posts.GetByIdAsync(postId);
        if (post == null) throw new InvalidOperationException("Post not found");
        attachment.PostId = postId;
        return await _attachments.AddAsync(attachment);
    }

    public async Task<IEnumerable<Attachment>> GetAttachmentsAsync(int postId)
    {
        var post = await _posts.GetByIdAsync(postId);
        if (post == null) throw new InvalidOperationException("Post not found");
        var attachments = await _attachments.GetAllAsync();
        return attachments.Where(a => a.PostId == postId);
    }
}
