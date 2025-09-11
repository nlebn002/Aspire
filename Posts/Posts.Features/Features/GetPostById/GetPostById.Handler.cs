using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Posts.Features.Shared.Dtos;
using Posts.Features.Shared.Errors;
using Posts.Infrastructure.Database;

namespace Posts.Features.Features.GetPostById;

internal class GetPostByIdHandler(PostsDbContext db, HybridCache cache) : IRequestHandler<GetPostByIdQuery, ErrorOr<PostDto>>
{
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
