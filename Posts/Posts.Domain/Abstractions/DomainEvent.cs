namespace Posts.Domain.Abstractions;

public record DomainEvent : IDomainEvent
{
    public DateTime OccuredOn { get; } = DateTime.UtcNow;
}
