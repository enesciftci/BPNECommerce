using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

namespace BPN.ECommerce.Infrastructure;

public interface ICircuitBreakerPolicyFactory
{
    IAsyncPolicy<HttpResponseMessage> Create();
}

public class CircuitBreakerPolicyFactory : ICircuitBreakerPolicyFactory
{
    private readonly ILogger _logger;

    public CircuitBreakerPolicyFactory(
        ILogger logger)
    {
        _logger = logger;
    }

    public IAsyncPolicy<HttpResponseMessage> Create()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(9, TimeSpan.FromSeconds(30),
                onBreak: (result, duration) =>
                {
                    _logger.LogWarning("Circuit breaker is open for {Duration}.", duration);
                },
                onReset: () =>
                {
                    _logger.LogInformation("Circuit breaker is reset. Everything is normal.");
                },
                onHalfOpen: () =>
                {
                    _logger.LogInformation("Circuit breaker is half open. Sending test request.");
                });
    }
}