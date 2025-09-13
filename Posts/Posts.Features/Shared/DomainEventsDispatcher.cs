using MediatR;
using Posts.Domain.Abstractions;
using Posts.Infrastructure.Database;

namespace Posts.Features.Shared.Events;

public sealed class DomainEventsDispatcher(PostsDbContext dbContext, IMediator mediator)
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
            await mediator.Publish(domainEvent, ct);
    }
}
