using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Posts.Infrastructure.Database;

namespace Posts.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddPostsInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var postgresConnectionString = configuration.GetConnectionString("postsDb");
        services.AddDbContext<PostsDbContext>(x =>x.UseNpgsql(postgresConnectionString));
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
        return services;
    }
}
