using Posts.Api.Routes;
using Posts.Features.Features.CreatePost;
using Posts.Features.Features.GetPostById;
using Shared.Api.Abstractions;

namespace Posts.Api.Endpoints
{
    public class PostEndpoints : IApiEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
        {
            routeBuilder.MapPost(RouteConsts.CreatePost, CreatePostHandler.HandleAsync);
            routeBuilder.MapGet(RouteConsts.GetPostById, GetPostByIdHandler.HandleAsync);
        }
    }
}
