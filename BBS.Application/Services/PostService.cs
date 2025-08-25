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

    public Task<Post> CreatePostAsync(Post post) => _repository.AddPostAsync(post);

    public Task<Comment> AddCommentAsync(int postId, Comment comment) => _repository.AddCommentAsync(postId, comment);

    public async Task<IEnumerable<Comment>> GetCommentsAsync(int postId) => await _repository.GetCommentsAsync(postId);
}
