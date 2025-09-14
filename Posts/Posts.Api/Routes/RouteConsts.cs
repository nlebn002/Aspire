namespace Posts.Api.Routes;

internal static class RouteConsts
{
    internal const string BaseRoute = "/api/posts";
    internal const string GetPostById = $"{BaseRoute}/{{postId}}";
    internal const string CreatePost = $"{BaseRoute}";
}
