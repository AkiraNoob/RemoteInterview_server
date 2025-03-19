using AuthService.Application.Interfaces;
using AuthService.Application.Request;
using AuthService.Application.Response;
using Core.Authorization;
using MessagingAPI.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Infrastructure.Auth.Jwt;
using Shared.Infrastructure.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthService.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;

    public TokenService(
        IOptions<JwtSettings> jwtSettings
        )
    {
        _jwtSettings = jwtSettings.Value;

    }
    public Task<TokenResponse> GetTokenAsync(VerifyUserLoginCredentialResponseMessage request, CancellationToken cancellationToken)
    {

    }
    public Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request);

    private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken(
           claims: claims,
           expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
           signingCredentials: signingCredentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }
    private string GenerateRefreshToken()
    {
        byte[] randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private string GenerateJwt(ApplicationUser user, string ipAddress) =>
    GenerateEncryptedToken(GetSigningCredentials(), GetClaims(user, ipAddress));

    private async Task<TokenResponse> GenerateTokens(ApplicationUser user, string ipAddress)
    {
        string token = GenerateJwt(user, ipAddress);

        //Token userToken = new()
        //{
        //    UserId = user.Id,
        //    RefreshToken = GenerateRefreshToken(),
        //    Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays),
        //    IpAddress = ipAddress
        //};

        //await _dbContext.Tokens.AddAsync(userToken);
        //await _dbContext.SaveChangesAsync();

        // if user has phone number and it's verified, and has email, and has fullname, and has academic rank, and has skipped password, then isInfoFullySubmitted = true
        bool isInfoFullySubmitted =
            !string.IsNullOrEmpty(user.PhoneNumber)
            && user.PhoneNumberConfirmed
            && !string.IsNullOrEmpty(user.Email)
            && !string.IsNullOrEmpty(user.FullName);

        return new TokenResponse(Token: token,RefreshToken: token, RefreshTokenExpiryTime: DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays));
    }

    private SigningCredentials GetSigningCredentials()
    {
        if (string.IsNullOrEmpty(_jwtSettings.Key))
        {
            throw new InvalidOperationException("No Key defined in JwtSettings config.");
        }

        byte[] secret = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
    }

    private IEnumerable<Claim> GetClaims(VerifyUserLoginCredentialResponseMessage user, string ipAddress) =>
    new List<Claim>
    {
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(InternalClaims.Fullname, $"{user.FullName}"),
            new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty)
    };
}
