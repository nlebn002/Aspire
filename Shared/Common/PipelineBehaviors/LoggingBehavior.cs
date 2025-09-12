using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Common.PipelineBehaviors
{
    public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            logger.LogInformation("Handling {RequestName} with payload: {@Request}", requestName, request);

            var response = await next();
            if (response is IErrorOr errorOr)
            {
                if (errorOr.IsError)
                {
                    logger.LogWarning("Request {RequestName} failed with errors: {Errors}",
                        typeof(TRequest).Name,
                        string.Join(", ", errorOr.Errors.Select(e => e.Description)));
                }
                else
                {
                    logger.LogInformation("Request {RequestName} succeeded", typeof(TRequest).Name);
                }
            }
            else
            {
                logger.LogInformation("Request {RequestName} returned {ResponseType}",
                    typeof(TRequest).Name, typeof(TResponse).Name);
            }

            return response;
        }
    }
}

