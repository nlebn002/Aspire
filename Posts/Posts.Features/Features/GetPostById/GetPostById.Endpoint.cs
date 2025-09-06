using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Posts.Domain.Entities;
using Posts.Features.Abstractions;
using Posts.Features.Features.GetPostById;
using Posts.Features.Shared.Dtos;
using Posts.Features.Shared.Errors;
using Posts.Features.Shared.Routes;

public record GetPostByIdQuery(Guid Id) : IRequest<ErrorOr<PostDto>>;

internal class GetPostById : IApiEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet(RouteConsts.GetPostById, Handle);
    }

    private static async Task<IResult> Handle(
        [FromRoute] string postId,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var post = await mediator.Send(new GetPostByIdQuery(Guid.Parse(postId)), cancellationToken);
        return post.ToResult();
    }
}

