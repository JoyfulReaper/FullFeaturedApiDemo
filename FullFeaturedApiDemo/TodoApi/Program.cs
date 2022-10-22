using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using TodoApi.HealthChecks;
using TodoApi.Options;
using TodoApi.ServiceSetup;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.AddAuthenticationAndAuthorization();
builder.AddAndSetupSwagger();
builder.AddAndSetupIdentity();
builder.AddAndSetupCors();
builder.AddTodoServices();

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

// Options
builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection(JwtOptions.Authentication));


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
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
}).AllowAnonymous();

// https://localhost:7131/healthchecks-ui
app.MapHealthChecksUI()
    .AllowAnonymous();

app.Run();
