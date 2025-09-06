using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Posts.Features.Shared.Dtos;
using Posts.Features.Shared.Errors;
using Posts.Infrastructure.Database;

namespace Posts.Features.Features.GetPostById;

internal class GetPostByIdHandler(PostsDbContext db) : IRequestHandler<GetPostByIdQuery, ErrorOr<PostDto>>
{
    public async Task<ErrorOr<PostDto>> Handle(GetPostByIdQuery request, CancellationToken ct)
    {
        var post = await db.Posts
            .Where(x => x.Id == request.Id)
            .Select(x => x.ToDto())
            .FirstOrDefaultAsync(ct);

        return post is null ?
            PostErrors.PostNotFound(request.Id) :
            post;
    }
}
