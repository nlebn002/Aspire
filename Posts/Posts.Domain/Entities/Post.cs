namespace Posts.Domain.Entities;

public sealed class Post
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Content { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }

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

        return new Post(Guid.NewGuid(), title, content);
    }

    // Domain behavior
    public void Update(string title, string content)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Content cannot be empty", nameof(content));

        Title = title;
        Content = content;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
