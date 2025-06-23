namespace MainService.Application.Slices.UserSlice.DTOs;

public record TokenDTO(string Token, string RefreshToken, DateTime RefreshTokenExpiryTime);

public record LoginTokenDTO(string Token, string RefreshToken, DateTime RefreshTokenExpiryTime, bool IsOnboarded, string UserId);