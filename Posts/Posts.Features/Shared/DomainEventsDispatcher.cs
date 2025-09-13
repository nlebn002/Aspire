using MediatR;
using Posts.Domain.Abstractions;
using Posts.Features.Abstractions;

namespace Posts.Features.Shared.Events;

public interface IDomainEventsDispatcher
{
    Task DispatchEventsAsync(CancellationToken ct);
}

public sealed class DomainEventsDispatcher(IPostsDbContext dbContext, IMediator mediator) : IDomainEventsDispatcher
{
    public async Task DispatchEventsAsync(CancellationToken ct)
    {
        var domainEntities = dbContext.ChangeTracker
            .Entries<AggregateRoot<Guid>>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var events = domainEntities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        foreach (var entity in domainEntities)
            entity.ClearDomainEvents();

        foreach (var domainEvent in events)
        {
            var notificationType = typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType());
            var notification = (INotification)Activator.CreateInstance(notificationType, domainEvent)!;
            await mediator.Publish(notification, ct);
        }
    }
}
