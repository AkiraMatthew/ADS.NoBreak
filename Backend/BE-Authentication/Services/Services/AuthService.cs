using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Services.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Services;

internal sealed class AuthService (JwtSettings jwtSettings,
        UserManager<User> userManager, ILogger<AuthService> logger, RoleManager<IdentityRole> roleManager): IAuthService
{
    public async Task<Token> GenerateTokenAsync(string username)
    {
        logger.LogInformation($"Generating token...");

        try
        {
            var user = await GetUser(username);
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);

            var userClaims = await GetUserClaims(user);
            var roles = await userManager.GetRolesAsync(user);
            var roleClaims = await GetRoleClaims(roles);

            var claims = new List<Claim>();
            string? userDataValue = null;

            foreach (var userClaim in userClaims)
            {
                if (userClaim.Type == ClaimTypes.UserData)
                {
                    userDataValue = userClaim.Value;
                }
                else
                {
                    claims.Add(new Claim("userClaims", $"{userClaim.Type}:{userClaim.Value}"));
                }
            }

            foreach (var roleClaim in roleClaims)
            {
                claims.Add(new Claim("roleClaims", $"{roleClaim.Type}:{roleClaim.Value}"));
            }

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            if (!string.IsNullOrEmpty(userDataValue))
            {
                claims.Add(new Claim(ClaimTypes.UserData, userDataValue));
            }

            claims.Add(new Claim("fullName", user.NomeCompleto ?? string.Empty));
            claims.Add(new Claim("userName", user.UserName ?? string.Empty));

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

            var securityToken = handler.CreateToken(securityTokenDescriptor);
            var encodedJwt = handler.WriteToken(securityToken);
            var refreshToken = await GenerateRefreshToken(user);

            logger.LogInformation($"Token generation success!");

            return new Token
            {
                AccessToken = encodedJwt,
                RefreshToken = refreshToken
            };
        }
        catch (BusinessException ex)
        {
            logger.LogError(ex, "An error occurred while generating the token for user: {Username}", username);
            throw new BusinessException($"An error occurred while generating the token for user: {username}", ex);
        }
    }
}
