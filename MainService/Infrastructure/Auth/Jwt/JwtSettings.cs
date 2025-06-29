namespace MainService.Infrastructure.Auth.Jwt;

public class JwtSettings
{
    public string? Key { get; set; }
    public int RefreshTokenExpirationInDays { get; set; }
    public int TokenExpirationInMinutes { get; set; }
}