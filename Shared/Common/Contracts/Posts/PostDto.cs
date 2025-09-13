
namespace Posts.Features.Shared.Dtos;

public record PostDto(Guid Id, string Title, string Content, DateTimeOffset CreatedAt);

