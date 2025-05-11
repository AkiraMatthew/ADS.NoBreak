using Domain.Entities;
using System.Security.Claims;

namespace Domain.Interfaces.Services;

public interface IClaimsService
{
    public Task<List<Claim>> GetClaimsForUserAsync(User user);
}
