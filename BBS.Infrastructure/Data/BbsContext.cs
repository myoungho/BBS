using BBS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BBS.Infrastructure.Data;

public class BbsContext : DbContext
{
    public BbsContext(DbContextOptions<BbsContext> options) : base(options) { }

    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Comment> Comments => Set<Comment>();
}
