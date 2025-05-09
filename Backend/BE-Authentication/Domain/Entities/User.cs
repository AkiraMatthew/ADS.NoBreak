using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User : IdentityUser
{
    public override string? PasswordHash { get; set; }
    public string? UserCountry{ get; set; }
}
