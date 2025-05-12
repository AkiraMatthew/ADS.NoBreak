using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace Login.Infra.CrossCutting.IoC.Configuration;

public class LoggingMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var transactionId = Guid.NewGuid().ToString();

        LogContext.PushProperty("UserIpAddress", context.Connection.RemoteIpAddress);
        LogContext.PushProperty("TransactionId", transactionId);

        await _next(context);
    }
}