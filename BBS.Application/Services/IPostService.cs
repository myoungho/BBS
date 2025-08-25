using System.Collections.Generic;

namespace BBS.Application.Services;

using BBS.Domain.Entities;

public interface IPostService
{
    Task<IEnumerable<Post>> GetPostsAsync();
    Task<Post?> GetPostAsync(int id);
    Task<Post> CreatePostAsync(Post post, string authorId);
    Task UpdatePostAsync(Post post, string userId);
    Task DeletePostAsync(int id, string userId);
    Task<Comment> AddCommentAsync(int postId, Comment comment);
    Task<IEnumerable<Comment>> GetCommentsAsync(int postId);
}
