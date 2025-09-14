using ErrorOr;
using Microsoft.AspNetCore.Http;
using Shared.Contracts.Exceptions;
using Shared.Extensions.Errors;
using System.Reflection;

namespace Shared.Api.Filters;

public class ErrorOrFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var result = await next(context);

        if (result is not IErrorOr)
            throw new InvalidTypeCustomException($"{nameof(ErrorOrFilter)} failed. Returned type is not {nameof(IErrorOr)}");

        var method = typeof(ErrorOrFilter)
                    .GetMethod(nameof(ToResult), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(result.GetType().GetGenericArguments()[0]);

        return method.Invoke(null, new[] { result });
    }

    private static IResult ToResult<T>(ErrorOr<T> errorOr) => errorOr.ToResult();
}
