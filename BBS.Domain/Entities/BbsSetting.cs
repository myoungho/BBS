namespace BBS.Domain.Entities;

public class BbsSetting
{
    public int Id { get; set; }
    public string AllowedFileExtensions { get; set; } = string.Empty;
}
