using System.Collections.Generic;
using BBS.Application.Services;
using BBS.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BBS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BbsSettingsController : ControllerBase
{
    private readonly IBbsSettingService _service;

    public BbsSettingsController(IBbsSettingService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BbsSetting>>> GetSettings()
    {
        var settings = await _service.GetSettingsAsync();
        return Ok(settings);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BbsSetting>> GetSetting(int id)
    {
        var setting = await _service.GetSettingAsync(id);
        if (setting == null) return NotFound();
        return Ok(setting);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<BbsSetting>> CreateSetting(BbsSetting setting)
    {
        try
        {
            var created = await _service.CreateSettingAsync(setting);
            return CreatedAtAction(nameof(GetSetting), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateSetting(int id, BbsSetting setting)
    {
        setting.Id = id;
        try
        {
            await _service.UpdateSettingAsync(setting);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteSetting(int id)
    {
        await _service.DeleteSettingAsync(id);
        return NoContent();
    }
}
