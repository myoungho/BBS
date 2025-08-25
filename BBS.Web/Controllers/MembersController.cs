using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using BBS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BBS.Web.Controllers;

public class MembersController : Controller
{
    private readonly IHttpClientFactory _factory;

    public MembersController(IHttpClientFactory factory)
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
        var users = await client.GetFromJsonAsync<List<User>>("api/users");
        return View(users ?? new List<User>());
    }

    public async Task<IActionResult> Details(string id)
    {
        var client = CreateClient();
        var user = await client.GetFromJsonAsync<User>($"api/users/{id}");
        if (user == null) return NotFound();
        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        if (HttpContext.Session.GetString("token") == null)
            return RedirectToAction("Login", "Account");
        var client = CreateClient();
        await client.DeleteAsync($"api/users/{id}");
        return RedirectToAction(nameof(Index));
    }
}
