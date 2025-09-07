using Common.Exceptions;
using ErrorOr;
using FluentValidation;
using MediatR;

namespace Common.Validation
{
    public sealed class ValidationBehavior<TRequest, TResult>(IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResult> where TRequest : notnull
    {
        public async Task<TResult> Handle(
            TRequest request,
            RequestHandlerDelegate<TResult> next,
            CancellationToken ct)
        {
            if (!validators.Any()) return await next(ct);

            var ctx = new ValidationContext<TRequest>(request);
            var failures = (await Task.WhenAll(validators.Select(v => v.ValidateAsync(ctx, ct))))
                .SelectMany(r => r.Errors)
                .Where(f => f is not null)
                .ToArray();

            if (failures.Length == 0) return await next(ct);

            var a = typeof(TResult);
            bool isErrorOr =
                a.IsGenericType &&
                a.GetGenericTypeDefinition() == typeof(ErrorOr<>);

            if (!isErrorOr)
                throw new InvalidTypeCustomException($"{nameof(ValidationBehavior<TRequest, TResult>)} failed. Returned type is not {nameof(IErrorOr)}");

            var errors = failures.Select(f => Error.Validation(f.PropertyName, f.ErrorMessage)).ToList();
            return (dynamic)errors;
        }
    }
}
