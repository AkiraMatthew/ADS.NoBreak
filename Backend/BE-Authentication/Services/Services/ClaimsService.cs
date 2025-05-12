using Domain.Entities;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Services.Services;

public class ClaimsService(
    UserManager<User> userManager, 
    RoleManager<IdentityRole> roleManager) 
    : IClaimsService
{
    public async Task<List<Claim>> GetClaimsForUserAsync(User user)
    {
        var claims = new List<Claim>();
        var userClaims = await userManager.GetClaimsAsync(user);
        var roles = await userManager.GetRolesAsync(user);
        var roleClaims = await GetRoleClaimsAsync(roles);

        string? userDataValue = null;

        foreach (var userClaim in userClaims)
        {
            if (userClaim.Type == ClaimTypes.UserData)
            {
                userDataValue = userClaim.Value;
            }
            else
            {
                claims.Add(new Claim(
                    "userClaims", 
                    $"{userClaim.Type}:{userClaim.Value}"));
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

        claims.Add(new Claim("fullName", user.FullName ?? string.Empty));
        claims.Add(new Claim("userName", user.UserName ?? string.Empty));

        return claims;
    }

    private async Task<IList<Claim>> GetRoleClaimsAsync(IEnumerable<string> roles)
    {
        var roleClaimsList = new List<Claim>();

        foreach (var roleName in roles)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                var roleClaims = await roleManager.GetClaimsAsync(role);
                if (roleClaims != null && roleClaims.Any())
                {
                    roleClaimsList.AddRange(roleClaims.Select(claim => new Claim(claim.Type, claim.Value)));
                }
            }
        }

        return roleClaimsList;
    }
}
