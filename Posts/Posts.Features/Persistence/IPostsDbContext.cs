using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Posts.Domain.Entities;

namespace Posts.Features.Persistence;

public interface IPostsDbContext
{
    public DbSet<Post> Posts { get; set; }
    ChangeTracker ChangeTracker { get; }
}
