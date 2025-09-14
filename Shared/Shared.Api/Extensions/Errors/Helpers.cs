using ErrorOr;
using Microsoft.AspNetCore.Http;

namespace Shared.Extensions.Errors;

public static class ErrorOrHttpResults
{
    public static IResult ToResult<T>(this ErrorOr<T> result) =>
        result.Match(
            value => Results.Ok(value),
            errors => Results.Problem(
                statusCode: MapToStatusCode(errors),
                title: errors.First().Code,          // short machine-readable summary
                detail: string.Join("; ", errors.Select(e => e.Description))));

    private static int MapToStatusCode(List<Error> errors)
    {
        // choose the highest-severity error; you can get fancy if you want
        var first = errors.First();
        return first.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}
