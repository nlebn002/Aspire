﻿using MediatR;
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
            try
            {
                var response = await next();
                logger.LogInformation("Handled {RequestName} with response: {@Response}", requestName, response);
                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error handling {RequestName} with payload {@Request}", requestName, request);
                throw;
            }
        }
    }
}
