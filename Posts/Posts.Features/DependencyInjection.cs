using Common.Validation;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Posts.Api.Extensions;
using Posts.Infrastructure;

namespace Posts.Features;

public static class DependencyInjection
{
    public static IServiceCollection AddPostsServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPostsInfrastructure(configuration);
        services.RegisterApiEndpointsFromAssemblyContaining(typeof(DependencyInjection));

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));
        });

        services.AddProblemDetails();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Posts.Features.Behaviors.LoggingBehavior<,>));



        return services;
    }
}
