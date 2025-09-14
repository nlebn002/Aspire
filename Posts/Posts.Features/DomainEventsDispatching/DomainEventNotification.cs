using MediatR;
using Posts.Domain.Abstractions;

namespace Posts.Features.Shared;

public sealed record DomainEventNotification<TDomainEvent>(TDomainEvent DomainEvent)
    : INotification where TDomainEvent : IDomainEvent;