using Microsoft.EntityFrameworkCore;
using Posts.Domain.Entities;
using Posts.Features.Abstractions;
using Posts.Features.Shared.Events;
namespace Posts.Infrastructure.Database;

public class PostsDbContext(DbContextOptions<PostsDbContext> options)
    : DbContext(options), IPostsDbContext
{
    public DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostsDbContext).Assembly);
    }
}
