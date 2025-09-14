using Posts.Api;
using Posts.Api.Extensions;
using Posts.Features;
using Posts.Infrastructure;
using Posts.Infrastructure.Seed;
using Serilog;
using Shared.Api.ExceptionHandlers;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddOpenApi();

builder.Services.AddPostsInfrastructure(builder.Configuration);
builder.Services.AddPostsFeaturesServices(builder.Configuration, mediatrCfg =>
{
    mediatrCfg.RegisterServicesFromAssemblies(
        [
            typeof(PostsFeaturesAssemblyMarker).Assembly,
            typeof(PostsInfrastructureAssemblyMarker).Assembly
        ]);
});

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.RegisterApiEndpointsFromAssemblyContaining(typeof(PostsApiAssemblyMarker));



if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<ISeedService, SeedService>();
    builder.Services.AddScoped<IMigrationService, MigrationService>();
}


var app = builder.Build();

if (app.Environment.IsDevelopment())
{

    app.MapOpenApi();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/openapi/v1.json", "Posts API v1");
        c.RoutePrefix = "swagger";
    });

    using (var scope = app.Services.CreateScope())
    {
        var seedService = scope.ServiceProvider.GetRequiredService<ISeedService>();
        var migrationService = scope.ServiceProvider.GetRequiredService<IMigrationService>();
        await migrationService.MigrateAsync(default);
        await seedService.SeedDataAsync(default);
    }
}

app.UseExceptionHandler();
app.MapApiEndpoints(app.Services);

app.Run();
