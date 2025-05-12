using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;

namespace Infrastructure.Configuration;
public sealed class JwtSecurityExtensionEvents(ILogger logger) : JwtBearerEvents
{
    public override async Task Challenge(JwtBearerChallengeContext context)
    {
        logger.Error("Invalid token, it can be expired or not informed.");
        await base.Challenge(context);
    }
}