using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Posts.Domain.Entities;
using Posts.Infrastructure.Database;

namespace Posts.Infrastructure.Seed;

public interface ISeedService
{
    Task SeedDataAsync(CancellationToken ct);
}

public class SeedService(PostsDbContext context, ILogger<SeedService> logger) : ISeedService
{
    public async Task SeedDataAsync(CancellationToken ct)
    {
        if (await context.Posts.AnyAsync(ct))
        {
            logger.LogInformation("Database has been already seeded");
            return;
        }

        logger.LogInformation("Start data seeding");
        await context.AddRangeAsync(await NewPostsAsync());
        await context.SaveChangesAsync(ct);
    }


    private Task<IEnumerable<Post>> NewPostsAsync()
    {
        IEnumerable<Post> posts = [
            Post.Create("Hello World", "Our very first seeded post."),
            Post.Create("Daily Update", "Quick summary of today’s work."),
            Post.Create("Bug Fixed", "Issue with API endpoint resolved."),
            Post.Create("Feature Added", "New comments feature released."),
            Post.Create("System Alert", "Scheduled maintenance tonight."),
            Post.Create("Team Meeting", "Reminder for tomorrow’s sync."),
            Post.Create("Release Notes", "Version 1.1 deployed to staging."),
            Post.Create("Feedback Wanted", "Share your thoughts on the UI."),
            Post.Create("Quick Tip", "Use shortcuts to speed up workflow."),
            Post.Create("Thank You", "Grateful for the team’s hard work.")
        ];

        return Task.FromResult(posts);
    }
}
