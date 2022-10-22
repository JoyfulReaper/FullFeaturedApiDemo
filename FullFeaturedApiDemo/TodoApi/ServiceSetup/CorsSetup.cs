using System.Runtime.CompilerServices;

namespace TodoApi.ServiceSetup;

public static class CorsSetup
{
    public static void AddAndSetupCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(opts =>
        {
            opts.AddDefaultPolicy(builder =>
                builder.WithOrigins("")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                );
        });
    }
}
