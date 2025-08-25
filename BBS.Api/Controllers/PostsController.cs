using System.Collections.Generic;
using BBS.Application.Services;
using BBS.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public async Task<ActionResult<Post>> CreatePost(Post post)
    {
        try
        {
            var userId = int.Parse(User.Identity!.Name!);
            var created = await _service.CreatePostAsync(post, userId);
            return CreatedAtAction(nameof(GetPost), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdatePost(int id, Post post)
    {
        var userId = int.Parse(User.Identity!.Name!);
        post.Id = id;
        try
        {
            await _service.UpdatePostAsync(post, userId);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeletePost(int id)
    {
        var userId = int.Parse(User.Identity!.Name!);
        try
        {
            await _service.DeletePostAsync(id, userId);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpPost("{postId}/comments")]
    [Authorize]
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
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{postId}/attachments")]
    [Authorize]
    public async Task<ActionResult<Attachment>> AddAttachment(int postId, Attachment attachment)
    {
        try
        {
            var created = await _service.AddAttachmentAsync(postId, attachment);
            return CreatedAtAction(nameof(GetPost), new { id = postId }, created);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{postId}/attachments")]
    public async Task<ActionResult<IEnumerable<Attachment>>> GetAttachments(int postId)
    {
        try
        {
            var attachments = await _service.GetAttachmentsAsync(postId);
            return Ok(attachments);
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

