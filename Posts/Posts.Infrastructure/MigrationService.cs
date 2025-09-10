using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Posts.Infrastructure.Database;

namespace Posts.Infrastructure;


public interface IMigrationService
{
    Task MigrateAsync(CancellationToken ct);
}

public class MigrationService(PostsDbContext context, ILogger<MigrationService> logger) : IMigrationService
{
    public async Task MigrateAsync(CancellationToken ct)
    {
        var pending = await context.Database.GetPendingMigrationsAsync();
        if (pending.Any())
        {
            logger.LogInformation($"Applying {pending.Count()} pending migrations: {string.Join(", ", pending)}");
            await context.Database.MigrateAsync(ct);
            logger.LogInformation($"Migrations have been applied");
        }
    }
}
