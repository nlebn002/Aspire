using Posts.Domain.Abstractions;
using Posts.Domain.Entities;

namespace Posts.Domain.Events;

public sealed record PostCreatedDomainEvent(Post Post) : DomainEvent;
public sealed record PostUpdatedDomainEvent(Post Post) : DomainEvent;
