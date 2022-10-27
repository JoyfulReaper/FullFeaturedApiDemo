using Microsoft.AspNetCore.Identity;

namespace TodoApi.Identity;

public class ApiIdentityUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}
