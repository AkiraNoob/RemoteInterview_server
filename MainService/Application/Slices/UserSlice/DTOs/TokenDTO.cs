namespace MainService.Application.Slices.UserSlice.DTOs;

public record TokenDTO(string Token, string RefreshToken, DateTime RefreshTokenExpiryTime);
