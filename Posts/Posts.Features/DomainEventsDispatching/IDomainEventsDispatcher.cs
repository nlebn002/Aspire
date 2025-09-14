namespace Posts.Features.Shared.Events;

public interface IDomainEventsDispatcher
{
    Task DispatchEventsAsync(CancellationToken ct);
}
