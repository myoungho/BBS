using System.Collections.Generic;

namespace BBS.Application.Services;

using BBS.Domain.Entities;

public interface IPostService
{
    Task<IEnumerable<Post>> GetPostsAsync();
    Task<Post?> GetPostAsync(int id);
    Task<Post> CreatePostAsync(Post post, int authorId);
    Task UpdatePostAsync(Post post, int userId);
    Task DeletePostAsync(int id, int userId);
    Task<Comment> AddCommentAsync(int postId, Comment comment);
    Task<IEnumerable<Comment>> GetCommentsAsync(int postId);
}
