using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Shared.Api.ExceptionHandlers;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IWebHostEnvironment env) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

        ProblemDetails problemDetails = new();
        var status = StatusCodes.Status500InternalServerError;

        if (env.IsDevelopment())
        {
            problemDetails = new()
            {
                Status = status,
                Title = exception.Message,
                Detail = exception.StackTrace
            };
        }
        else
        {
            var (st, title, detail) = Map(exception);
            status = st;
            problemDetails = new()
            {
                Status = status,
                Title = title,
                Detail = detail
            };
        }

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
