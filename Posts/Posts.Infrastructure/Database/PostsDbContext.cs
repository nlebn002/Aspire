
using Microsoft.EntityFrameworkCore;
using Posts.Domain.Entities;
namespace Posts.Infrastructure.Database;

public class PostsDbContext(DbContextOptions<PostsDbContext> options) : DbContext(options)
{
    public DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostsDbContext).Assembly);
    }
}
