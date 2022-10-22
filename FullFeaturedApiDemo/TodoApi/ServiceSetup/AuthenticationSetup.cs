using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TodoApi.ServiceSetup;

public static class AuthenticationSetup
{
    public static void AddAuthenticationAndAuthorization(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer(opts =>
            {
                opts.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration.GetValue<string>("Authentication:Issuer"),
                    ValidAudience = builder.Configuration.GetValue<string>("Authentication:Audience"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("Authentication:SecretKey")))
                };
            });

        builder.Services.AddAuthorization(opts =>
        {
            opts.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });
    }
}
