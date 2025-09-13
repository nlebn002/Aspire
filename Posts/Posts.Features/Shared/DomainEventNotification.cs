using MediatR;
using Posts.Domain.Abstractions;
using Posts.Domain.Events;

namespace Posts.Features.Shared;

public sealed record DomainEventNotification<TDomainEvent>(TDomainEvent DomainEvent)
    : INotification where TDomainEvent : IDomainEvent;