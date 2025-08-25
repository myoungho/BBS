using System.Collections.Generic;
using BBS.Application.Services;
using BBS.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BBS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;

    public UsersController(IUserService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = await _service.GetUsersAsync();
        return Ok(users);
    }

    [HttpGet("{loginId}")]
    public async Task<ActionResult<User>> GetUser(string loginId)
    {
        var user = await _service.GetUserAsync(loginId);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpDelete("{loginId}")]
    [Authorize]
    public async Task<IActionResult> DeleteUser(string loginId)
    {
        await _service.DeleteUserAsync(loginId);
        return NoContent();
    }
}
