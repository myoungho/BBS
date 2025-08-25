using System.Collections.Generic;

namespace BBS.Domain.Repositories;

using BBS.Domain.Entities;

public interface IPostRepository
{
    Task<List<Post>> GetPostsAsync();
    Task<Post?> GetPostAsync(int id);
    Task<Post> AddPostAsync(Post post);
    Task UpdatePostAsync(Post post);
    Task DeletePostAsync(int id);
    Task<Comment> AddCommentAsync(int postId, Comment comment);
    Task<List<Comment>> GetCommentsAsync(int postId);
}
