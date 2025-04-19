using Domain.DTO;
using System.Net;

namespace Domain.Interfaces.Services;

public interface IUserService
{
    Task<string> SignUpAsync(NetworkCredential credentials, SignUpDTO signUp);
}
