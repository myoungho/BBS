using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using BBS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BBS.Web.Controllers;

public class PostsController : Controller
{
    private readonly HttpClient _client;

    public PostsController(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("BbsApi");
    }

    public async Task<IActionResult> Index()
    {
        var posts = await _client.GetFromJsonAsync<List<Post>>("api/posts");
        return View(posts ?? new List<Post>());
    }
}
