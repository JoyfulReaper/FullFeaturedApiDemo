using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using TodoApi.Identity;

namespace MinimalApi.EndPoints;

public static class AuthenticationEndpoint
{
    public record AuthenticationData(string? UserName, string? Password);

    public static void AddAuthenticationEndPoints(this WebApplication app)
    {
        app.MapPost("/api/token", async (IConfiguration config, 
            [FromBody] AuthenticationData data, 
            SignInManager<ApiIdentityUser> signingManager,
            UserManager<ApiIdentityUser> userManager) =>
        {
            var user = await ValidateCredentials(data, signingManager, userManager);

            if (user is null)
            {
                return Results.Unauthorized();
            }

            var token = GenerateToken(user, config);
            return Results.Ok(token);
        }).AllowAnonymous();
    }

    private static string GenerateToken(IdentityUser user, IConfiguration config)
    {
        var secretKey = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes(
                config.GetValue<string>("Jwt:SecretKey")));

        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = new();
        claims.Add(new(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
        claims.Add(new(JwtRegisteredClaimNames.UniqueName, user.UserName));


        var token = new JwtSecurityToken(
            config.GetValue<string>("Jwt:Issuer"),
            config.GetValue<string>("Jwt:Audience"),
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(1),
            signingCredentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async static Task<ApiIdentityUser?> ValidateCredentials(AuthenticationData auth, 
        SignInManager<ApiIdentityUser> signInManager, 
        UserManager<ApiIdentityUser> userManager)
    {
        ApiIdentityUser user = await userManager.FindByNameAsync(auth.UserName);
        SignInResult result = await signInManager.CheckPasswordSignInAsync(user, auth.Password, true);

        if (result.Succeeded)
        {
            return user;
        }

        return null;
    }
}
