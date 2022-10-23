using TodoApi.HealthChecks;

namespace TodoApi.ServiceSetup;

public static class HealthCheckSetup
{
    public static void AddAndSetupHealthChecks(this WebApplicationBuilder builder)
    {
        // Pre-made Health Checks and such
        // https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks
        builder.Services.AddHealthChecks()
            .AddCheck<DemoHealthCheck>("Demo Health Check");

        // AspNetCore.HealthChecks.UI
        builder.Services.AddHealthChecksUI(opts =>
        {
            opts.AddHealthCheckEndpoint("api", "/health");
            opts.SetEvaluationTimeInSeconds(30);
            opts.SetMinimumSecondsBetweenFailureNotifications(60);
        }).AddInMemoryStorage();
    }
}
