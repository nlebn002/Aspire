using Microsoft.AspNetCore.Routing;

namespace Shared.Api.Abstractions;

public interface IApiEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder routeBuilder);
}
