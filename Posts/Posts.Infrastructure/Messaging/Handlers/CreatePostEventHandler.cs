using Confluent.Kafka;
using MediatR;
using Posts.Domain.Events;
using Posts.Features.Shared;
using Shared.Contracts;
using Shared.Contracts.Posts;
using System.Text.Json;

namespace Posts.Infrastructure.Messaging.Handlers;

public class CreatePostEventHandler(IProducer<Null, string> producer)
    : INotificationHandler<DomainEventNotification<PostCreatedDomainEvent>>
{
    public async Task Handle(DomainEventNotification<PostCreatedDomainEvent> notification, CancellationToken cancellationToken)
    {
        notification.DomainEvent.Deconstruct(out var post);

        var message = new PostCreatedIntegrationEvent(post.Id, post.Title, post.Content);
        var payload = JsonSerializer.Serialize(message);

        await producer.ProduceAsync(
            IntegrationTopics.PostCreated,
            new Message<Null, string> { Value = payload },
            cancellationToken);
    }
}