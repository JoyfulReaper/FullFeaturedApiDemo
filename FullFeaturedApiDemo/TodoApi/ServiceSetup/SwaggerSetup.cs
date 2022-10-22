using Microsoft.OpenApi.Models;

namespace TodoApi.ServiceSetup;

public static class SwaggerSetup
{
    public static void AddAndSetupSwagger(this WebApplicationBuilder builder)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(opts =>
        {
            var title = "Fully Featured Demo API";
            var description = "Demo API demonstrating the usage of Identity, JWT Bearer Tokens, API Versioning, Health Checks, Rate Limiting and probably more!";
            var terms = new Uri("https://opensource.org/licenses/MIT");
            var License = new OpenApiLicense()
            {
                Name = "MIT",
                Url = new Uri("https://opensource.org/licenses/MIT"),
            };

            opts.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1 (deprecated)",
                Title = $"{title} v1",
                Description = description,
                TermsOfService = terms,
                License = License
            });

            opts.SwaggerDoc("v2", new OpenApiInfo
            {
                Version = "v2",
                Title = $"{title} v2",
                Description = description,
                TermsOfService = terms,
                License = License
            });
        });

        builder.Services.AddApiVersioning(opts =>
        {
            opts.AssumeDefaultVersionWhenUnspecified = true;
            opts.DefaultApiVersion = new(1, 0);
            opts.ReportApiVersions = true;
        });

        builder.Services.AddVersionedApiExplorer(opts =>
        {
            opts.GroupNameFormat = "'v'VVV";
            opts.SubstituteApiVersionInUrl = true;
        });
    }
}
