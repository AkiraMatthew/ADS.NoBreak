using Domain;
using Domain.DTO;
using Domain.Entities;
using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;

namespace Services.Services;

public class UserService(
    UserManager<User> userManager,
    IAuthService authService,
    ILogger logger,
    IHttpContextAccessor httpContextAccessor) : IUserService
{
    public async Task<Token> SignInAsync(NetworkCredential credentials)
    {
        try
        {
            var user = await userManager.FindByNameAsync(credentials.UserName)
                ?? throw new Exception($"User {credentials.UserName} not found!");

            var isLockedOut = await userManager.GetLockoutEnabledAsync(user);

            if (isLockedOut)
                throw new Exception($"User {credentials.UserName} is blocked!");

            string password = credentials.Password;

            var isAValidPwd = await userManager.CheckPasswordAsync(user, password);
            if (!isAValidPwd)
            {
                logger.LogInformation("Invalid password for user {UserName}", credentials.UserName);

                await userManager.AccessFailedAsync(user);
                throw new Exception("Invalid password!");
            }

            await userManager.ResetAccessFailedCountAsync(user);

            var userName = user.UserName;
            if (string.IsNullOrEmpty(userName))
            {
                throw new Exception("User name is missing!");
            }

            var token = await authService.GenerateTokenAsync(userName.ToLower());

            InsertTokenIntoCookies(user.Id, token);

            logger.LogInformation("Signing successful for user {UserName}", credentials.UserName);

            return token;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while signing in the user.");
            throw new Exception(ex.Message);
        }
    }

    public async Task<string> SignUpAsync(NetworkCredential credentials, SignUpDTO createUser)
    {
        logger.LogInformation("Creating new user...");
        try
        {
            var user = new User
            {
                Email = createUser.Email.ToLower(),
                UserName = credentials.UserName.ToLower(),
                UserCountry = createUser.Country?.ToLower()
            };

            if (await UserExists(user.UserName))
                throw new Exception($"User already exists!");

            var response = await userManager.CreateAsync(user, credentials.Password);
            if (!response.Succeeded)
                throw new Exception($"Couldn't create a new user!");

            var token = await authService.GenerateTokenAsync(credentials.UserName.ToLower());

            var confirmEmailToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodeEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
            var validEmailToken = WebEncoders.Base64UrlEncode(encodeEmailToken);

            logger.LogInformation("User {UserName} created successfully!", credentials.UserName);

            InsertTokenIntoCookies(user.Id, token);
            return validEmailToken;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while creating a new user: {UserName}",
                credentials.UserName);
            throw new Exception(ex.Message);
        }
    }

    private async Task<bool> UserExists(string userName)
    {
        return await userManager.FindByNameAsync(userName) is not null;
    }

    private void InsertTokenIntoCookies(string userId, Token? token)
    {
        var cookie = GetCookieOptions();

        if (httpContextAccessor?.HttpContext?.Response == null)
            throw new Exception("HttpContext or Response Cookies is not available!");

        if (string.IsNullOrWhiteSpace(token?.AccessToken))
            throw new Exception("Access token is null or empty!");

        if (string.IsNullOrWhiteSpace(token?.RefreshToken))
            throw new Exception("Refresh token is null or empty!");

        httpContextAccessor.HttpContext?.Response?.Cookies.Append(JwtToken.ACCESS_TOKEN, token.AccessToken, cookie);
        httpContextAccessor.HttpContext?.Response?.Cookies.Append(JwtToken.REFRESH_TOKEN, token.RefreshToken, cookie);
        httpContextAccessor.HttpContext?.Response?.Cookies.Append(JwtToken.USER, userId, cookie);
    }

    private static CookieOptions GetCookieOptions() =>
        new()
        {
            Expires = DateTimeOffset.Now.AddMinutes(15),
            HttpOnly = true,
            Path = "/",
            Secure = true,
            SameSite = SameSiteMode.None
        };
}
