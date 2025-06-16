using AuthService.Domain.Models;
using MainService.Application.Exceptions;
using MainService.Application.Helpers;
using MainService.Application.Slices.UserSlice.DTOs;
using MainService.Application.Slices.UserSlice.Interfaces;
using MainService.Domain.Authorization;
using MainService.Infrastructure.Auth.Jwt;
using MainService.Infrastructure.Identity;
using MainService.Infrastructure.Persistence.Context;
using MainService.Infrastructure.Persistence.Persistence.Extension;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MainService.Infrastructure.Services;

public class TokenService(
    UserManager<ApplicationUser> userManager,
    IOptions<JwtSettings> jwtSettings,
    ApplicationDbContext dbContext
        ) : ITokenService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<TokenDTO> GetTokenAsync(TokenRequest request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FilterByUserIdentifier(request.Email).FirstOrDefaultAsync(cancellationToken)
                        ?? throw new NotFoundException("User not found");

        //if (request.Email.IsEmail() && !user.EmailConfirmed)
        //{
        //    throw new UnauthorizedException("Email not verified");
        //}

        bool isPasswordValid = !string.IsNullOrWhiteSpace(user.Password) && BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
        if (!isPasswordValid)
        {
            throw new UnauthorizedException("Invalid password");
        }

        return await GenerateTokens(user);
    }
    public async Task<TokenDTO> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var userPrincipal = GetPrincipalFromExpiredToken(request.Token);
        string? userId = userPrincipal.GetUserId() ?? throw new UnauthorizedException("User not found");

        var user = await _userManager.FindByIdAsync(userId) ?? throw new UnauthorizedException("User not found");

        var userToken = await _dbContext.Token.FirstOrDefaultAsync(x => x.UserId == user.Id.ToString() && x.RefreshToken == request.RefreshToken, cancellationToken) ?? throw new UnauthorizedException("Refresh token not found");
        if (userToken.Expires <= DateTime.UtcNow)
        {
            await _dbContext.SaveChangesAsync();
            throw new UnauthorizedException("Refresh token expired");
        }

        return await GenerateTokens(user);
    }

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

    private string GenerateJwt(ApplicationUser user) =>
    GenerateEncryptedToken(GetSigningCredentials(), GetClaims(user));

    private async Task<TokenDTO> GenerateTokens(ApplicationUser user)
    {
        string token = GenerateJwt(user);

        Token userToken = new()
        {
            UserId = user.Id.ToString(),
            RefreshToken = GenerateRefreshToken(),
            Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays),
        };

        await _dbContext.Token.AddAsync(userToken);
        await _dbContext.SaveChangesAsync();

        return new TokenDTO(Token: token, RefreshToken: token, RefreshTokenExpiryTime: DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays));
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

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        if (string.IsNullOrEmpty(_jwtSettings.Key))
        {
            throw new InvalidOperationException("No Key defined in JwtSettings config.");
        }

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = false
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new UnauthorizedException("Invalid token");
        }

        return principal;
    }

    private IEnumerable<Claim> GetClaims(ApplicationUser user) =>
    new List<Claim>
    {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email),
            new(InternalClaims.Fullname, $"{user.FullName}"),
            new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty)
    };
}
