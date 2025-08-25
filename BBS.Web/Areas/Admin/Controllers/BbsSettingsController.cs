using System.Net.Http.Headers;
using System.Net.Http.Json;
using BBS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BBS.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class BbsSettingsController : Controller
{
    private readonly IHttpClientFactory _factory;

    public BbsSettingsController(IHttpClientFactory factory)
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
        var settings = await client.GetFromJsonAsync<List<BbsSetting>>("api/bbssettings");
        return View(settings ?? new List<BbsSetting>());
    }

    public async Task<IActionResult> Details(int id)
    {
        var client = CreateClient();
        var setting = await client.GetFromJsonAsync<BbsSetting>($"api/bbssettings/{id}");
        if (setting == null) return NotFound();
        return View(setting);
    }

    [HttpGet]
    public IActionResult Create()
    {
        if (HttpContext.Session.GetString("token") == null)
            return RedirectToAction("Login", "Account", new { area = "" });
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(BbsSetting setting)
    {
        if (HttpContext.Session.GetString("token") == null)
            return RedirectToAction("Login", "Account", new { area = "" });
        var client = CreateClient();
        var response = await client.PostAsJsonAsync("api/bbssettings", setting);
        if (response.IsSuccessStatusCode)
            return RedirectToAction(nameof(Index));
        return View(setting);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        if (HttpContext.Session.GetString("token") == null)
            return RedirectToAction("Login", "Account", new { area = "" });
        var client = CreateClient();
        var setting = await client.GetFromJsonAsync<BbsSetting>($"api/bbssettings/{id}");
        if (setting == null) return NotFound();
        return View(setting);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, BbsSetting setting)
    {
        if (HttpContext.Session.GetString("token") == null)
            return RedirectToAction("Login", "Account", new { area = "" });
        var client = CreateClient();
        var response = await client.PutAsJsonAsync($"api/bbssettings/{id}", setting);
        if (response.IsSuccessStatusCode)
            return RedirectToAction(nameof(Index));
        return View(setting);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        if (HttpContext.Session.GetString("token") == null)
            return RedirectToAction("Login", "Account", new { area = "" });
        var client = CreateClient();
        await client.DeleteAsync($"api/bbssettings/{id}");
        return RedirectToAction(nameof(Index));
    }
}
