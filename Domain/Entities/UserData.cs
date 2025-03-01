using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class UserData : IdentityUser
{
    public required string PasswordHash { get; set; } = string.Empty;
    public override string? Email { get; set; }
    public string? CellphoneNumber { get; set; }
}
