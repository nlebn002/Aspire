using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Posts.Features.Errors;
using Posts.Features.Persistence;
using Shared.Contracts.Posts;

namespace Posts.Features.Features.GetPostById;
public record GetPostByIdQuery(Guid Id) : IRequest<ErrorOr<PostDto>>;
public class GetPostByIdHandler(IPostsDbContext db, HybridCache cache) : IRequestHandler<GetPostByIdQuery, ErrorOr<PostDto>>
{
    public static async Task<ErrorOr<PostDto>> HandleAsync(
        [FromRoute] string postId,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var post = await mediator.Send(new GetPostByIdQuery(Guid.Parse(postId)), cancellationToken);
        return post;
    }

    public async Task<ErrorOr<PostDto>> Handle(GetPostByIdQuery request, CancellationToken ct)
    {
        var response = await cache.GetOrCreateAsync(
            $"post-{request.Id}",
            async token =>
            {
                var post = await db.Posts
                    .Where(x => x.Id == request.Id)
                    .Select(x => x.ToDto())
                    .FirstOrDefaultAsync(ct);

                return post;
            });

        return response is null ?
            PostErrors.PostNotFound(request.Id) :
            response;
    }
}
