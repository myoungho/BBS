using System.Collections.Generic;
using BBS.Domain.Entities;
using BBS.Domain.Repositories;

namespace BBS.Application.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _repository;

    public PostService(IPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Post>> GetPostsAsync() => await _repository.GetPostsAsync();

    public Task<Post?> GetPostAsync(int id) => _repository.GetPostAsync(id);

    public Task<Post> CreatePostAsync(Post post, string authorId)
    {
        post.AuthorId = authorId;
        return _repository.AddPostAsync(post);
    }

    public async Task<bool> UpdatePostAsync(Post post, string userId)
    {
        var existing = await _repository.GetPostAsync(post.Id);
        if (existing == null || existing.AuthorId != userId) return false;
        existing.Title = post.Title;
        existing.Content = post.Content;
        await _repository.UpdatePostAsync(existing);
        return true;
    }

    public async Task<bool> DeletePostAsync(int id, string userId)
    {
        var existing = await _repository.GetPostAsync(id);
        if (existing == null || existing.AuthorId != userId) return false;
        await _repository.DeletePostAsync(id);
        return true;
    }

    public Task<Comment> AddCommentAsync(int postId, Comment comment) => _repository.AddCommentAsync(postId, comment);

    public async Task<IEnumerable<Comment>> GetCommentsAsync(int postId) => await _repository.GetCommentsAsync(postId);
}
