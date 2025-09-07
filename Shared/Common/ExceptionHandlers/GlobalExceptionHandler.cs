using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Common.ExceptionHandlers;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

        var (status, title, detail) = Map(exception);
        var problemDetails = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail
        };

        httpContext.Response.StatusCode = status;
        httpContext.Response.ContentType = "application/problem+json";
        var res = await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken)
            .ContinueWith(_ => true, cancellationToken);

        return res;
    }

    private static (int Status, string Title, string? Detail) Map(Exception ex)
    {
        return (StatusCodes.Status500InternalServerError, "Internal Server Error",
                  "An unexpected error occurred. Please try again later.");
    }

}
