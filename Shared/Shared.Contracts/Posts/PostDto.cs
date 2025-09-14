
namespace Shared.Contracts.Posts;

public record PostDto(Guid Id, string Title, string Content, DateTimeOffset CreatedAt);

