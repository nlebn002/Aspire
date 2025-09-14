namespace Shared.Contracts;

public record IntegrationEvent
{
    public DateTime OccuredOn { get; } = DateTime.UtcNow;
}
