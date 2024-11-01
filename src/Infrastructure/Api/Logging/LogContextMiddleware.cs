using CraftersCloud.Core.AspNetCore.Filters;
using CraftersCloud.ReferenceArchitecture.Domain.Identity;
using Microsoft.AspNetCore.Http;
using Serilog.Context;
using Serilog.Core;
using Serilog.Core.Enrichers;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Api.Logging;

[UsedImplicitly]
public class LogContextMiddleware(RequestDelegate next)
{
    [UsedImplicitly]
    public async Task InvokeAsync(HttpContext context)
    {
        using (LogContext.Push(CreateEnrichers(context)))
        {
            await next.Invoke(context);
        }
    }

    private static ILogEventEnricher[] CreateEnrichers(HttpContext context) =>
    [
        new PropertyEnricher("User", GetCurrentUserId(context)!),
        new PropertyEnricher("Address", context.Connection.RemoteIpAddress!)
    ];

    private static Guid? GetCurrentUserId(HttpContext context)
    {
        var userId = context.Resolve<ICurrentUserProvider>().User?.UserId;
        return userId;
    }
}