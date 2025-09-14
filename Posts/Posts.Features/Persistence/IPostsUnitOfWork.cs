namespace Posts.Features.Persistence;

public interface IPostsUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task<int> SaveChangesWithRaisingEventsAsync(CancellationToken ct = default);
}
