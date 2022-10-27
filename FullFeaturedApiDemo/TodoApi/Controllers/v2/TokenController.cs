// With thanks to: https://code-maze.com/using-refresh-tokens-in-asp-net-core-authentication/

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Identity;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers.v2;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("2.0")]
[ApiController]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class TokenController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApiIdentityUser> _userManager;

    public TokenController(ITokenService tokenService,
        UserManager<ApiIdentityUser> userManager)
	{
        _tokenService = tokenService;
        _userManager = userManager;
    }

    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Refresh(TokenApiModel tokenApiModel)
    {
        if (tokenApiModel?.RefreshToken is null ||
            tokenApiModel?.AccessToken is null)
        {
            return BadRequest("Invalid client request");
        }

        string accessToken = tokenApiModel.AccessToken;
        string refreshToken = tokenApiModel.RefreshToken;

        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
        var username = principal.Identity!.Name;
        var user = await _userManager.FindByNameAsync(username);

        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return BadRequest("Invalid client request");
        }

        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
        var newRefreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7); // TODO: Put in app settings

        await _userManager.UpdateAsync(user);
        return Ok(new AuthenticatedResponse()
        {
            Token = newAccessToken,
            RefreshToken = newRefreshToken
        });
    }

    [HttpPost]
    [Route("revoke")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task <IActionResult> Revoke()
    {
        var username = User.Identity!.Name;
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            return BadRequest();
        }

        user.RefreshToken = null;
        await _userManager.UpdateAsync(user);

        return NoContent();
    }
}
