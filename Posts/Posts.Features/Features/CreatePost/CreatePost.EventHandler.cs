using Common.Contracts;
using Common.Contracts.Posts;
using Confluent.Kafka;
using MediatR;
using Posts.Domain.Events;
using System.Text.Json;

namespace Posts.Features.Features.CreatePost;

public sealed record PostCreatedNotification(PostCreatedDomainEvent DomainEvent) : INotification;
public class CreatePostEventHandler(IProducer<Null, string> producer) : INotificationHandler<PostCreatedNotification>
{
    public async Task Handle(PostCreatedNotification notification, CancellationToken cancellationToken)
    {
        notification.DomainEvent.Deconstruct(out var post);
        PostCreatedIntegrationEvent message = new(post.Id, post.Title, post.Content);
        var payload = JsonSerializer.Serialize(message);
        await producer.ProduceAsync(IntegrationTopics.PostCreated, new() { Value = payload }, cancellationToken);
    }
}
