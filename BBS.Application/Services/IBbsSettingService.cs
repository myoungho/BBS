namespace BBS.Application.Services;

using BBS.Domain.Entities;

public interface IBbsSettingService
{
    Task<List<BbsSetting>> GetSettingsAsync();
    Task<BbsSetting?> GetSettingAsync(int id);
    Task<BbsSetting> CreateSettingAsync(BbsSetting setting);
    Task UpdateSettingAsync(BbsSetting setting);
    Task DeleteSettingAsync(int id);
}
