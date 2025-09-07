using Common.ExceptionHandlers;
using Posts.Api.Extensions;
using Posts.Features;
using Posts.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddPostsInfrastructure(builder.Configuration);
builder.Services.AddPostsServices(builder.Configuration);
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/openapi/v1.json", "Posts API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseExceptionHandler();
app.MapApiEndpoints(app.Services);
app.Run();
