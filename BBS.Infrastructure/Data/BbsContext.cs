using BBS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BBS.Infrastructure.Data;

public class BbsContext : DbContext
{
    public BbsContext(DbContextOptions<BbsContext> options) : base(options) { }

    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<Attachment> Attachments => Set<Attachment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().HasKey(u => u.Id);
        modelBuilder.Entity<Post>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(p => p.AuthorId);
        modelBuilder.Entity<Attachment>()
            .HasOne(a => a.Post)
            .WithMany(p => p.Attachments)
            .HasForeignKey(a => a.PostId);

        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);
    }
}

