using Common.PipelineBehaviors;
using Common.Validation;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Posts.Api.Extensions;
using Posts.Features.Shared.Events;

namespace Posts.Features;

public static class PostFeaturesDependencyInjection
{
    public static IServiceCollection AddPostsFeaturesServices(this IServiceCollection services, 
        IConfiguration configuration, 
        Action<MediatRServiceConfiguration> configure)
    {
        // Register all IApiEndpoint implementations from this assembly
        services.RegisterApiEndpointsFromAssemblyContaining(typeof(PostsFeaturesAssemblyMarker));
        services.AddValidatorsFromAssembly(typeof(PostFeaturesDependencyInjection).Assembly);
        services.AddMediatR(configure);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();
        return services;
    }
}
