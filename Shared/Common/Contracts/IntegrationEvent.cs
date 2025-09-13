namespace Common.Contracts;

public record IntegrationEvent
{
    public DateTime OccuredOn { get; } = DateTime.UtcNow;
}
