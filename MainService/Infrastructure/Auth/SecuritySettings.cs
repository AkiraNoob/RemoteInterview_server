namespace MainService.Infrastructure.Auth;

public class SecuritySettings
{
    public AllowedIssuer[]? AllowedIssuers { get; set; }
    public string? Provider { get; set; }
    public bool RequireConfirmedAccount { get; set; }

    public class AllowedIssuer
    {
        public string? Issuer { get; set; }
        public string? Key { get; set; }
    }
}