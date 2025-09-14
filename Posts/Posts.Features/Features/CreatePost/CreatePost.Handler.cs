using ErrorOr;
using MediatR;
using Posts.Domain.Entities;
using Posts.Features.Abstractions;
using Posts.Features.Shared.Dtos;

namespace Posts.Features.Features.CreatePost;

public class CreatePostHandler(IPostsDbContext context, IPostsUnitOfWork uow) : IRequestHandler<CreatePostCommand, ErrorOr<PostDto>>
{
    public async Task<ErrorOr<PostDto>> Handle(
        CreatePostCommand request,
        CancellationToken cancellationToken)
    {
        var post = Post.Create(request.Title, request.Content);
        context.Posts.Add(post);
        await uow.SaveChangesWithRaisingEventsAsync(cancellationToken);

        return post.ToDto();
    }
}
