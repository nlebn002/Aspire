namespace Shared.Contracts.Posts;

public sealed record class PostCreatedIntegrationEvent(Guid Id, string Title, string Content) : IntegrationEvent;
