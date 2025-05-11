using Domain.Entities;
using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Services.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Services;

public class AuthService (
    JwtSettings jwtSettings,
    UserManager<User> userManager,
    ILogger<AuthService> logger, 
    IClaimsService claimsService
    )
    : IAuthService
{
    private const string ACCESS_TOKEN_TYPE = "at+jwt";

    public async Task<Token> GenerateTokenAsync(string username)
    {
        logger.LogInformation($"Generating token...");

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);

            var user = await GetUserDataAsync(username);
            var claims = await claimsService.GetClaimsForUserAsync(user);

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtSettings.Issuer,
                Audience = jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(double.Parse(jwtSettings.ExpiresAt)),
                IssuedAt = DateTime.UtcNow,
                TokenType = ACCESS_TOKEN_TYPE,
                Subject = identityClaims
            };

            var securityToken = tokenHandler.CreateToken(securityTokenDescriptor);
            var encodedJwt = tokenHandler.WriteToken(securityToken);
            //var refreshToken = await GenerateRefreshToken(user);

            logger.LogInformation($"Token generation success!");

            return new Token
            {
                AccessToken = encodedJwt,
                //RefreshToken = refreshToken
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while generating the token for user: {Username}", username);
            throw new Exception($"An error occurred while generating the token for user: {username}", ex);
        }
    }

    private async Task<User> GetUserDataAsync(string username)
    {
        var userData = await userManager.FindByNameAsync(username);
        return userData ?? throw new Exception("User not found");
    }
}
