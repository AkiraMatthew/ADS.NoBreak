using Domain.DTO;
using Domain.Entities;
using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Services.Services;

public class UserService(UserManager<User> userManager) : IUserService
{
    public async Task<Token> SignInAsync(NetworkCredential credentials)
    {
        try
        {
            
        }
        catch (Exception ex) { }
    }

    public async Task<string> SignUpAsync(NetworkCredential credentials, SignUpDTO signUp)
    {
        try
        {
            var user = new User
            {
                Email = signUp.Email,
                UserName = credentials.UserName.ToLower(),
                UserCountry = signUp.Country
            };

            if(await Exists(user.UserName))
                throw new Exception($"User already exists!");

            var response = await userManager.CreateAsync(user, credentials.Password);
            
            if (!response.Succeeded)
                throw new Exception($"Couldn't create a new user!");

            var token = await 
        }
        catch (Exception ex) { }
    }

    private async Task<bool> Exists(string userName)
    {
        return await userManager.FindByNameAsync(userName) is not null;
    }
}
