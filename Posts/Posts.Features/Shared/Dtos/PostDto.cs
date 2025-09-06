using Posts.Domain.Entities;

namespace Posts.Features.Shared.Dtos;

public record PostDto(Guid Id, string Title, string Content, DateTimeOffset CreatedAt);

public static class PostDtoExtensions
{
    public static PostDto ToDto(this Post post) =>
        new PostDto(post.Id, post.Title, post.Content, post.CreatedAtUtc);
}