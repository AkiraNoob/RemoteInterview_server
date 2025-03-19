namespace AuthService.Application.Request;

public record RefreshTokenRequest(string Token, string RefreshToken);