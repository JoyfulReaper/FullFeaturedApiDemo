using TodoApi.Options;
using TodoApi.ServiceSetup;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.AddAuthenticationAndAuthorization();
builder.AddAndSetupSwagger();
builder.AddAndSetupIdentity();
builder.AddAndSetupCors();

// Options
builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection(JwtOptions.Authentication));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
