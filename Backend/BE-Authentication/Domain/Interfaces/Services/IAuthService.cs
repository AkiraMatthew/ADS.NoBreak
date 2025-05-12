using Domain.Models;

namespace Domain.Interfaces.Services;

public interface IAuthService
{
    Task<Token> GenerateTokenAsync(string username);
}
