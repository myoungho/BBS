using Bbs.Core.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bbs.Mvc.Controllers;

public class PostsController : Controller
{
    private readonly BbsContext _context;

    public PostsController(BbsContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var posts = await _context.Posts.Include(p => p.Comments).ToListAsync();
        return View(posts);
    }
}
