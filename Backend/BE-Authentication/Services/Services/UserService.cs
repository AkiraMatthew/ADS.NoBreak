using Domain.DTO;
using Domain.Entities;
using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Services.Services;

public class UserService(UserManager<User> userManager, ILogger logger, AuthService authService) : IUserService
{
    public async Task<Token> SignInAsync(NetworkCredential credentials)
    {
        try
        {
            
        }
        catch (Exception ex) { }
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
                throw new Exception( $"Couldn't create a new user!");

            var token = await authService.GenerateTokenAsync(credentials.UserName.ToLower());

            await userManager.AddToRoleAsync(user, RoleName.Vendedor);

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
}
