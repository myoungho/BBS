using System.Net.Http.Headers;
using System.Net.Http.Json;
using BBS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BBS.Web.Controllers;

public class PostsController : Controller
{
    private readonly IHttpClientFactory _factory;

    public PostsController(IHttpClientFactory factory)
    {
        _factory = factory;
    }

    private HttpClient CreateClient()
    {
        var client = _factory.CreateClient("BbsApi");
        var token = HttpContext.Session.GetString("token");
        if (!string.IsNullOrEmpty(token))
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    public async Task<IActionResult> Index()
    {
        var client = CreateClient();
        var posts = await client.GetFromJsonAsync<List<Post>>("api/posts");
        return View(posts ?? new List<Post>());
    }

    public async Task<IActionResult> Details(int id)
    {
        var client = CreateClient();
        var post = await client.GetFromJsonAsync<Post>($"api/posts/{id}");
        if (post == null) return NotFound();
        return View(post);
    }

    [HttpGet]
    public IActionResult Create()
    {
        if (HttpContext.Session.GetString("token") == null)
            return RedirectToAction("Login", "Account");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Post post)
    {
        if (HttpContext.Session.GetString("token") == null)
            return RedirectToAction("Login", "Account");
        var client = CreateClient();
        var response = await client.PostAsJsonAsync("api/posts", post);
        if (response.IsSuccessStatusCode)
            return RedirectToAction(nameof(Index));
        return View(post);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        if (HttpContext.Session.GetString("token") == null)
            return RedirectToAction("Login", "Account");
        var client = CreateClient();
        var post = await client.GetFromJsonAsync<Post>($"api/posts/{id}");
        if (post == null) return NotFound();
        return View(post);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Post post)
    {
        if (HttpContext.Session.GetString("token") == null)
            return RedirectToAction("Login", "Account");
        var client = CreateClient();
        var response = await client.PutAsJsonAsync($"api/posts/{id}", post);
        if (response.IsSuccessStatusCode)
            return RedirectToAction(nameof(Index));
        return View(post);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        if (HttpContext.Session.GetString("token") == null)
            return RedirectToAction("Login", "Account");
        var client = CreateClient();
        await client.DeleteAsync($"api/posts/{id}");
        return RedirectToAction(nameof(Index));
    }
}
