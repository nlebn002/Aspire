namespace Posts.Domain.Abstractions;

public interface IDomainEvent
{
    DateTime OccuredOn { get; }
}
