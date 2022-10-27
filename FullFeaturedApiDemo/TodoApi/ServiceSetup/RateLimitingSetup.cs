using AspNetCoreRateLimit;

namespace TodoApi.ServiceSetup;

public static class RateLimitingSetup
{
    // Rate Limiting: https://github.com/stefanprodan/AspNetCoreRateLimit
    public static void AddRateLimiting(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
        builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        builder.Services.AddInMemoryRateLimiting();
    }
}
