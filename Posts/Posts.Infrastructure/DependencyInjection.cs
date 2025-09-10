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
        services.AddDbContext<PostsDbContext>(x =>
        {
            x.UseNpgsql(postgresConnectionString);
        });
        return services;
    }
}
