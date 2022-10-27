using AspNetCoreRateLimit;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MinimalApi.EndPoints;
using TodoApi.EndPoints;
using TodoApi.Options;
using TodoApi.Services;
using TodoApi.ServiceSetup;

var builder = WebApplication.CreateBuilder(args);

// Options
builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection(JwtOptions.Jwt));

builder.Services.AddControllers();
builder.Services.AddMemoryCache();

builder.AddSwagger();
builder.AddIdentity();
builder.AddAuthenticationAndAuthorization();
builder.AddCors();
builder.AddTodoServices();
builder.AddHealthChecks();
builder.AddRateLimiting();

builder.Services.AddTransient<ITokenService, TokenService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opts =>
    {
        opts.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
        opts.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Minimal Endpoint example
app.AddAuthenticationEndPoints();
app.AddTodoEndpoints();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
}).AllowAnonymous();

// https://localhost:7131/healthchecks-ui
app.MapHealthChecksUI()
    .AllowAnonymous();

app.UseIpRateLimiting();

app.Run();
