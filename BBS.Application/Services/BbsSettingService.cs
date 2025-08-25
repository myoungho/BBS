using System;
using System.Collections.Generic;
using BBS.Domain.Entities;
using BBS.Domain.Repositories;

namespace BBS.Application.Services;

public class BbsSettingService : IBbsSettingService
{
    private readonly IRepository<BbsSetting> _repository;

    public BbsSettingService(IRepository<BbsSetting> repository)
    {
        _repository = repository;
    }

    public async Task<List<BbsSetting>> GetSettingsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public Task<BbsSetting?> GetSettingAsync(int id)
    {
        return _repository.GetByIdAsync(id);
    }

    public async Task<BbsSetting> CreateSettingAsync(BbsSetting setting)
    {
        if (string.IsNullOrWhiteSpace(setting.AllowedFileExtensions))
            throw new ArgumentException("Allowed file extensions are required", nameof(setting));
        return await _repository.AddAsync(setting);
    }

    public async Task UpdateSettingAsync(BbsSetting setting)
    {
        if (string.IsNullOrWhiteSpace(setting.AllowedFileExtensions))
            throw new ArgumentException("Allowed file extensions are required", nameof(setting));
        var existing = await _repository.GetByIdAsync(setting.Id);
        if (existing == null) throw new KeyNotFoundException("Setting not found");
        existing.AllowedFileExtensions = setting.AllowedFileExtensions;
        await _repository.UpdateAsync(existing);
    }

    public async Task DeleteSettingAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}
