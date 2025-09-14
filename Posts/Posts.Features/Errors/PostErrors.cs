using ErrorOr;

namespace Posts.Features.Errors;

internal static class PostErrors
{
    public static Error PostNotFound(Guid id) => Error.NotFound(
            code: "Post.NotFound",
            description: $"Post with ID '{id}' was not found."
        );
}
