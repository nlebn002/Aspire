using Posts.Domain.Abstractions;
using Posts.Domain.Events;

namespace Posts.Domain.Entities;

public sealed class Post : AggregateRoot<Guid>
{
    public string Title { get; private set; } = null!;
    public string Content { get; private set; } = null!;


    // Factory method (instead of public ctor, for enforcing invariants)
    private Post() { } // For EF Core

    private Post(Guid id, string title, string content)
    {
        Id = id;
        Title = title;
        Content = content;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public static Post Create(string title, string content)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Content cannot be empty", nameof(content));

        var post = new Post(Guid.NewGuid(), title, content);

        post.AddDomainEvent(new PostCreatedDomainEvent(post));

        return post;
    }

    public void Update(string title, string content)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Content cannot be empty", nameof(content));

        Title = title;
        Content = content;
        UpdatedAtUtc = DateTime.UtcNow;

        AddDomainEvent(new PostUpdatedDomainEvent(this));
    }
}
