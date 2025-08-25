using System.Collections.Generic;
using System.Linq;
using BBS.Domain.Entities;
using BBS.Domain.Repositories;
using BBS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BBS.Infrastructure.Repositories;

public class PostRepository : Repository<Post>, IPostRepository
{
    public PostRepository(BbsContext context) : base(context) { }

    public async Task<List<Post>> GetPostsAsync() => await GetAllAsync();

    public async Task<Post?> GetPostAsync(int id) => await GetByIdAsync(id);

    public async Task<Post> AddPostAsync(Post post) => await AddAsync(post);

    public async Task UpdatePostAsync(Post post) => await UpdateAsync(post);

    public async Task DeletePostAsync(int id) => await DeleteAsync(id);

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

    public override async Task<List<Post>> GetAllAsync()
    {
        return await _context.Posts.Include(p => p.Comments).ToListAsync();
    }

    public override async Task<Post?> GetByIdAsync(int id)
    {
        return await _context.Posts.Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}
