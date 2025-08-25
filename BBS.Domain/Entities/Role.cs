using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BBS.Domain.Entities;

public class Role
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = default!;

    public List<UserRole> UserRoles { get; set; } = new();
}

