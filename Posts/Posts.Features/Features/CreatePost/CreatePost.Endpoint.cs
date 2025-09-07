using Common.Extensions.Errors;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Posts.Features.Abstractions;
using Posts.Features.Shared.Dtos;
using Posts.Features.Shared.Routes;

namespace Posts.Features.Features.CreatePost;

public record CreatePostCommand(string Title, string Content) : IRequest<ErrorOr<PostDto>>;
public class CreatePost : IApiEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPost(RouteConsts.CreatePost, Handle);
    }

    public async static Task<IResult> Handle([FromBody] CreatePostDto createPostDto, IMediator mediator)
    {
        var post = await mediator.Send(new CreatePostCommand(createPostDto.Title, createPostDto.Content));
        
        return post.ToResult();
    }
}
