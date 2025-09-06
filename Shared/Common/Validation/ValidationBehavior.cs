using ErrorOr;
using FluentValidation;
using MediatR;
using ValidationException = FluentValidation.ValidationException;

namespace Common.Validation
{
    public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken ct)
        {
            if (!validators.Any())
                return await next();

            var ctx = new ValidationContext<TRequest>(request);
            var results = await Task.WhenAll(validators.Select(v => v.ValidateAsync(ctx, ct)));
            var failures = results.SelectMany(r => r.Errors).Where(f => f is not null).ToArray();

            if (failures.Length > 0)
                throw new ValidationException(failures);

            return await next();
        }
    }

    public sealed class ValidationWrapperBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken ct)
        {

            try
            {
                next.Invoke();
            }
            catch (ValidationException ex)
            {
                var a = request.ToErrorOr().Er;
                ErrorOr.Error.Validation()
            }
        }
    }
}
