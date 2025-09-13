using Posts.Features.Abstractions;
using Posts.Features.Shared.Events;

namespace Posts.Infrastructure.Database;

/// <summary>
/// Is needed to ensure that domain events are dispatched after saving changes to the database.
/// </summary>
/// <param name="dbContext"></param>
/// <param name="dispatcher"></param>
public sealed class EFUnitOfWork(PostsDbContext dbContext, IDomainEventsDispatcher dispatcher) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var result = await dbContext.SaveChangesAsync(ct);
        await dispatcher.DispatchEventsAsync(ct);
        return result;
    }
}
