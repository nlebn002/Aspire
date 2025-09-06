using Posts.Api.Extensions;
using Posts.Features;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
builder.Services.AddPostsServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(c =>
    {
        // tell Swagger UI to use the built-in doc
        c.SwaggerEndpoint("/openapi/v1.json", "Posts API v1");
        c.RoutePrefix = "swagger"; // UI at /swagger);
    });
}

app.MapApiEndpoints(app.Services);
app.Run();
