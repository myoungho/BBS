using BBS.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace BBS.Core.Data;

public class BbsContext : DbContext
{
    public BbsContext(DbContextOptions<BbsContext> options) : base(options) { }

    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Comment> Comments => Set<Comment>();
}
