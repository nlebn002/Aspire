using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Posts.Features.Abstractions;
using Posts.Infrastructure.Database;

namespace Posts.Infrastructure;

public static class PostInfrastructureDependencyInjection
{
    public static IServiceCollection AddPostsInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var postgresConnectionString = configuration.GetConnectionString("postsDb");
        services.AddDbContext<PostsDbContext>(x => x.UseNpgsql(postgresConnectionString));
        services.AddScoped<IPostsDbContext>(x => x.GetRequiredService<PostsDbContext>());
        services.AddScoped<IPostsUnitOfWork, PostsUnitOfWork>();

        services.AddHybridCache(o =>
        {
            o.MaximumKeyLength = 1000;
            o.DefaultEntryOptions = new()
            {
                Expiration = TimeSpan.FromMinutes(5),
            };
        });

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("redis");
            options.InstanceName = "Posts.Redis";
        });

        services.AddSingleton<IProducer<Null, string>>(x =>
        {
            var kafkaProducerConfig = new ProducerConfig { BootstrapServers = configuration.GetConnectionString("kafka") };
            return new ProducerBuilder<Null, string>(kafkaProducerConfig).Build();
        });

        return services;
    }

}
