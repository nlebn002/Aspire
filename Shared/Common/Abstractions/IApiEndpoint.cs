using Microsoft.AspNetCore.Routing;

namespace Posts.Features.Abstractions;

public interface IApiEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder routeBuilder);
}
