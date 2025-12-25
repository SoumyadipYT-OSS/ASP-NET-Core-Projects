using MediatR;

namespace IdentityService.Application.Behaviors;


public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly Serilog.ILogger _logger;

    public LoggingBehavior(Serilog.ILogger logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var correlationId = Guid.NewGuid();

        _logger.Information("Executing request {RequestName} with correlation ID: {CorrelationId}", requestName, correlationId);

        try
        {
            var response = await next();
            _logger.Information("Request {RequestName} completed successfully with correlation ID: {CorrelationId}", requestName, correlationId);
            return response;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Request {RequestName} failed with correlation ID: {CorrelationId}", requestName, correlationId);
            throw;
        }
    }
}
