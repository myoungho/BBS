using System.ComponentModel.DataAnnotations;

namespace BBS.Domain.Entities;

public class User
{
    [Key]
    public string Id { get; set; } = default!; // Email as ID

    [Required]
    public string Nickname { get; set; } = default!;

    [Required]
    public string PasswordHash { get; set; } = default!;
}

