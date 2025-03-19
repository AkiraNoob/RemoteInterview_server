namespace AuthService.Application.Response;

public record TokenResponse(string Token, string RefreshToken, DateTime RefreshTokenExpiryTime);
