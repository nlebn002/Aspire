namespace Posts.Domain.Abstractions;

public abstract class AggregateRoot<T> : Entity<T> where T : struct
{
    protected AggregateRoot() : base() { }
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public void AddDomainEvent(IDomainEvent @event) => _domainEvents.Add(@event);
    public void ClearDomainEvents() => _domainEvents.Clear();
}
