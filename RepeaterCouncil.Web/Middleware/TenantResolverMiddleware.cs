using Microsoft.Extensions.Caching.Memory;
using RepeaterCouncil.Web.Data;

namespace RepeaterCouncil.Web.Middleware
{
    public class TenantResolverMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantResolverMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IMemoryCache cache, ApplicationDbContext db)
        {
            var host = context.Request.Host.Host;

            var tenant = cache.GetOrCreate($"tenant_{host}", entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return db.Tenants.FirstOrDefault(t => t.Url == host);
            });

            if (tenant == null)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("These are not the droids you're looking for.");
                return;
            }

            context.Items["Tenant"] = tenant;
            context.Items["TenantName"] = tenant.Name;

            await _next(context);
        }
    }

}
