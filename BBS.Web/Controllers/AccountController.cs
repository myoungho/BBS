using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace BBS.Web.Controllers;

public class AccountController : Controller
{
    private readonly IHttpClientFactory _factory;

    public AccountController(IHttpClientFactory factory)
    {
        _factory = factory;
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var client = _factory.CreateClient("BbsApi");
        var response = await client.PostAsJsonAsync("api/auth/register", dto);
        if (response.IsSuccessStatusCode)
            return RedirectToAction("Login");
        ModelState.AddModelError(string.Empty, "Registration failed");
        return View(dto);
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var client = _factory.CreateClient("BbsApi");
        var response = await client.PostAsJsonAsync("api/auth/login", dto);
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Login failed");
            return View(dto);
        }
        var result = await response.Content.ReadFromJsonAsync<LoginResult>();
        if (result?.token == null)
        {
            ModelState.AddModelError(string.Empty, "Login failed");
            return View(dto);
        }
        HttpContext.Session.SetString("token", result.token);
        var userId = GetUserIdFromToken(result.token);
        if (userId != null)
            HttpContext.Session.SetString("userId", userId);
        HttpContext.Session.SetString("user", dto.Email);
        return RedirectToAction("Index", "Posts");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Remove("token");
        HttpContext.Session.Remove("user");
        HttpContext.Session.Remove("userId");
        return RedirectToAction("Index", "Posts");
    }

    private static string? GetUserIdFromToken(string token)
    {
        var parts = token.Split('.');
        if (parts.Length < 2) return null;
        var payload = parts[1];
        switch (payload.Length % 4)
        {
            case 2: payload += "=="; break;
            case 3: payload += "="; break;
        }
        var bytes = Convert.FromBase64String(payload.Replace('-', '+').Replace('_', '/'));
        using var doc = JsonDocument.Parse(bytes);
        if (doc.RootElement.TryGetProperty("unique_name", out var prop))
            return prop.GetString();
        return null;
    }
}

public record RegisterDto(string Email, string Password, string Nickname);
public record LoginDto(string Email, string Password);
public record LoginResult(string token);
