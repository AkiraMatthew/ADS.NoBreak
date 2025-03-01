using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Contexts;

public class LoginContextBase: IdentityDbContext<UserData>
{

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserData>().Property(u => u.UserName).HasMaxLength(12);

        builder.HasDefaultSchema("identity");
    }
}