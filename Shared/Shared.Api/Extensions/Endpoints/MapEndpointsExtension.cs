using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared.Api.Abstractions;
using Shared.Api.Filters;

namespace Posts.Api.Extensions;

public static class MapEndpointsExtension
{
    public static IServiceCollection RegisterApiEndpointsFromAssemblyContaining(this IServiceCollection services, Type marker)
    {
        var assembly = marker.Assembly;

        var endpointTypes = assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IApiEndpoint)) && t is { IsClass: true, IsAbstract: false, IsInterface: false });

        var serviceDescriptors = endpointTypes
            .Select(type => ServiceDescriptor.Transient(typeof(IApiEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);
        return services;
    }

    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder routeBuilder, IServiceProvider serviceProvider)
    {

        var endpoints = serviceProvider.GetRequiredService<IEnumerable<IApiEndpoint>>();
        var api = routeBuilder.MapGroup("").AddEndpointFilter<ErrorOrFilter>();
        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(api);
        }

        return routeBuilder;
    }
}
