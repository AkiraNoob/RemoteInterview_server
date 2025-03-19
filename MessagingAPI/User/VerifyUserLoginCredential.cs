namespace MessagingAPI.User;

public record VerifyUserLoginCredentialRequestMessage
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public record VerifyUserLoginCredentialResponseMessage
{
    public string Email { get; set; }
    public string FullName { get; set; }
    public string Role { get; set; }
    public string? PhoneNumber { get; set; }
    public Guid UserId { get; set; }
}

