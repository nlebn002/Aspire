using ErrorOr;
using MediatR;
using Posts.Domain.Entities;
using Posts.Features.Shared.Dtos;
using Posts.Infrastructure.Database;

namespace Posts.Features.Features.CreatePost;

public class CreatePostHandler(PostsDbContext db) : IRequestHandler<CreatePostCommand, ErrorOr<PostDto>>
{
    public async Task<ErrorOr<PostDto>> Handle(
        CreatePostCommand request,
        CancellationToken cancellationToken)
    {
        var post = Post.Create(request.Title, request.Content);
        db.Add(post);
        await db.SaveChangesAsync(cancellationToken);
        return post.ToDto();
    }
}
