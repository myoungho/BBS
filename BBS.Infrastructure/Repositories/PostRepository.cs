using System.Collections.Generic;
using BBS.Domain.Entities;
using BBS.Domain.Repositories;
using BBS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BBS.Infrastructure.Repositories;

public class PostRepository : IPostRepository
{
    private readonly BbsContext _context;

    public PostRepository(BbsContext context)
    {
        _context = context;
    }

    public async Task<List<Post>> GetPostsAsync()
    {
        return await _context.Posts.Include(p => p.Comments).ToListAsync();
    }

    public async Task<Post?> GetPostAsync(int id)
    {
        return await _context.Posts.Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Post> AddPostAsync(Post post)
    {
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();
        return post;
    }

    public async Task<Comment> AddCommentAsync(int postId, Comment comment)
    {
        var post = await _context.Posts.FindAsync(postId);
        if (post == null)
        {
            throw new InvalidOperationException("Post not found");
        }
        comment.PostId = postId;
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();
        return comment;
    }

    public async Task<List<Comment>> GetCommentsAsync(int postId)
    {
        var post = await _context.Posts.FindAsync(postId);
        if (post == null)
        {
            throw new InvalidOperationException("Post not found");
        }
        return await _context.Comments.Where(c => c.PostId == postId).ToListAsync();
    }
}
