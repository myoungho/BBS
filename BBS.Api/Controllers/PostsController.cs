using System.Collections.Generic;
using BBS.Application.Services;
using BBS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BBS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostService _service;

    public PostsController(IPostService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
    {
        var posts = await _service.GetPostsAsync();
        return Ok(posts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Post>> GetPost(int id)
    {
        var post = await _service.GetPostAsync(id);
        if (post == null)
        {
            return NotFound();
        }
        return Ok(post);
    }

    [HttpPost]
    public async Task<ActionResult<Post>> CreatePost(Post post)
    {
        var created = await _service.CreatePostAsync(post);
        return CreatedAtAction(nameof(GetPost), new { id = created.Id }, created);
    }

    [HttpPost("{postId}/comments")]
    public async Task<ActionResult<Comment>> AddComment(int postId, Comment comment)
    {
        try
        {
            var created = await _service.AddCommentAsync(postId, comment);
            return CreatedAtAction(nameof(GetPost), new { id = postId }, created);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    [HttpGet("{postId}/comments")]
    public async Task<ActionResult<IEnumerable<Comment>>> GetComments(int postId)
    {
        try
        {
            var comments = await _service.GetCommentsAsync(postId);
            return Ok(comments);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
}
