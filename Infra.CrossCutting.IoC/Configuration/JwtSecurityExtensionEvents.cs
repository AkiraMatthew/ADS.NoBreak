using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;

namespace Infra.CrossCutting.IoC.Configuration;
public sealed class JwtSecurityExtensionEvents : JwtBearerEvents
{
    private readonly ILogger<JwtSecurityExtensionEvents> _logger;

    public JwtSecurityExtensionEvents(ILogger<JwtSecurityExtensionEvents> logger)
    {
        _logger = logger;
    }

    public override async Task Challenge(JwtBearerChallengeContext context)
    {
        _logger.LogError("Invalid token, it can be expired or not informed.");
        await base.Challenge(context);
    }
}