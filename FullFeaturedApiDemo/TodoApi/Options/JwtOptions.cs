namespace TodoApi.Options;

public class JwtOptions
{
    public const string Authentication = "JwtOptions";

    public string SecretKey { get; set; } = String.Empty;
    public string Issuer { get; set; } = String.Empty;
    public string Audience { get; set; } = String.Empty;
}
