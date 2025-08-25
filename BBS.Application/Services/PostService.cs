using System;
using System.Collections.Generic;
using System.Linq;
using BBS.Domain.Entities;
using BBS.Domain.Repositories;

namespace BBS.Application.Services;

public class PostService : IPostService
{
    private readonly IRepository<Post, int> _posts;
    private readonly IRepository<Comment, int> _comments;

    public PostService(IRepository<Post, int> posts, IRepository<Comment, int> comments)
    {
        _posts = posts;
        _comments = comments;
    }

    public async Task<IEnumerable<Post>> GetPostsAsync() => await _posts.GetAllAsync();

    public Task<Post?> GetPostAsync(int id) => _posts.GetByIdAsync(id);

    public Task<Post> CreatePostAsync(Post post, string authorId)
    {
        post.AuthorId = authorId;
        return _posts.AddAsync(post);
    }

    public async Task<bool> UpdatePostAsync(Post post, string userId)
    {
        var existing = await _posts.GetByIdAsync(post.Id);
        if (existing == null || existing.AuthorId != userId) return false;
        existing.Title = post.Title;
        existing.Content = post.Content;
        await _posts.UpdateAsync(existing);
        return true;
    }

    public async Task<bool> DeletePostAsync(int id, string userId)
    {
        var existing = await _posts.GetByIdAsync(id);
        if (existing == null || existing.AuthorId != userId) return false;
        await _posts.DeleteAsync(id);
        return true;
    }

    public async Task<Comment> AddCommentAsync(int postId, Comment comment)
    {
        var post = await _posts.GetByIdAsync(postId);
        if (post == null) throw new InvalidOperationException("Post not found");
        comment.PostId = postId;
        return await _comments.AddAsync(comment);
    }

    public async Task<IEnumerable<Comment>> GetCommentsAsync(int postId)
    {
        var comments = await _comments.GetAllAsync();
        return comments.Where(c => c.PostId == postId);
    }
}
