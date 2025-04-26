using Domain.DTO;
using Domain.Models;
using System.Net;

namespace Domain.Interfaces.Services;

public interface IUserService
{
    Task<string> SignUpAsync(NetworkCredential credentials, SignUpDTO signUp);
    Task<Token> SignInAsync(NetworkCredential credentials);
}
