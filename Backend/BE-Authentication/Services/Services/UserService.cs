using Domain.DTO;
using Domain.Interfaces.Services;
using Domain.Models;
using System.Net;

namespace Services.Services;

public class UserService : IUserService
{
    public Task<Token> SignInAsync(NetworkCredential credentials)
    {
        throw new NotImplementedException();
    }

    public Task<string> SignUpAsync(NetworkCredential credentials, SignUpDTO signUp)
    {
        throw new NotImplementedException();
    }
}
