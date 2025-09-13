using Common.PipelineBehaviors;
using Common.Validation;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Posts.Api.Extensions;
using Posts.Features.Shared.Events;

namespace Posts.Features;

public static class DependencyInjection
{
    public static IServiceCollection AddPostsServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register all IApiEndpoint implementations from this assembly
        services.RegisterApiEndpointsFromAssemblyContaining(typeof(DependencyInjection));
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection)));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();

        return services;
    }
}
