using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TodoApi.Identity;
using TodoApi.Models;
using TodoApi.Services;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace TodoApi.Controllers.v2;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("2.0")]
[ApiController]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class AuthenticationController : ControllerBase
{
    private readonly UserManager<ApiIdentityUser> _userManager;
    private readonly SignInManager<ApiIdentityUser> _signInManager;
    private readonly ITokenService _tokenService;

    public record RegistrationRecord(string? UserName, string? Password, string? EmailAddress);
    public record LoginRecord(string? UserName, string? Password);

    public AuthenticationController(UserManager<ApiIdentityUser> userManager,
        SignInManager<ApiIdentityUser> signInManager,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    [HttpPost("token", Name = "GetToken")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [AllowAnonymous]
    public async Task<ActionResult<string>> Authenticate([FromBody] LoginRecord loginRec)
    {
        var user = await ValidateCredentials(loginRec);

        if (user is null)
        {
            return Unauthorized();
        }

        var token = GenerateToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7); // TODO: put in appsettings
        await _userManager.UpdateAsync(user);

        return Ok(new AuthenticatedResponse
        {
            Token = token,
            RefreshToken = refreshToken,
        });
    }

    [AllowAnonymous]
    [HttpPost("register", Name = "Register")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegistrationRecord auth)
    {
        var user = new ApiIdentityUser
        {
            UserName = auth.UserName,
            Email = auth.EmailAddress,
        };

        var result = await _userManager.CreateAsync(user, auth.Password);
        if (result.Succeeded)
        {
            return NoContent();
        }
        else
        {
            var errorOut = string.Empty;
            foreach (var error in result.Errors)
            {
                errorOut += " " + error.Description;
            }
            return BadRequest(errorOut);
        }
    }

    private string GenerateToken(ApiIdentityUser user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        return _tokenService.GenerateAccessToken(claims);
    }

    private async Task<ApiIdentityUser?> ValidateCredentials(LoginRecord loginRec)
    {
        ApiIdentityUser user = await _userManager.FindByNameAsync(loginRec.UserName);
        if(user is null)
        {
            return null;
        }

        SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, loginRec.Password, true);
        if (result.Succeeded)
        {
            return user;
        }

        return null;
    }
}
