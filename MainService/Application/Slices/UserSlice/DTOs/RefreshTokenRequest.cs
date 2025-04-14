namespace MainService.Application.Slices.UserSlice.DTOs;

public record RefreshTokenRequest(string Token, string RefreshToken);