using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Posts.Domain.Entities;
using Posts.Features.Persistence;
using Shared.Contracts.Posts;

namespace Posts.Features.Features.CreatePost;

public record CreatePostCommand(string Title, string Content) : IRequest<ErrorOr<PostDto>>;

public class CreatePostHandler(IPostsDbContext context, IPostsUnitOfWork uow) : IRequestHandler<CreatePostCommand, ErrorOr<PostDto>>
{
    public async static Task<ErrorOr<PostDto>> HandleAsync([FromBody] CreatePostDto createPostDto, IMediator mediator)
    {
        var post = await mediator.Send(new CreatePostCommand(createPostDto.Title, createPostDto.Content));
        return post;
    }

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
