using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TodoApi.HealthChecks;

public class DemoHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, 
        CancellationToken cancellationToken = default)
    {
        int responseTimeInMs = Random.Shared.Next(400);
        if(responseTimeInMs < 100)
        {
            return Task.FromResult(HealthCheckResult.Healthy( $"The response time is amazing: {responseTimeInMs} ms"));
        }
        else if (responseTimeInMs < 200)
        {
            return Task.FromResult(HealthCheckResult.Degraded($"The response time is degraded: {responseTimeInMs} ms"));
        }
        else
        {
            return Task.FromResult(HealthCheckResult.Unhealthy($"The response time is horrible: {responseTimeInMs} ms"));
        }
    }
}
