using Bbs.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Bbs.Api.Data;

public class BbsContext : DbContext
{
    public BbsContext(DbContextOptions<BbsContext> options) : base(options) { }

    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Comment> Comments => Set<Comment>();
}
