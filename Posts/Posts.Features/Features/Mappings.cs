using Posts.Domain.Entities;
using Posts.Features.Shared.Dtos;

namespace Posts.Features.Features;

public static class Mappings
{
    public static PostDto ToDto(this Post post) =>
        new PostDto(post.Id, post.Title, post.Content, post.CreatedAtUtc);
}
