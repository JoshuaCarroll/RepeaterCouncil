using Microsoft.AspNetCore.Identity;
using RepeaterCouncil.Web.Models;

namespace RepeaterCouncil.Web.Middleware
{
    public class EmailConfirmationMiddleware
    {
        private readonly RequestDelegate _next;

        public EmailConfirmationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            // Skip middleware for anonymous users
            if (!context.User.Identity.IsAuthenticated)
            {
                await _next(context);
                return;
            }

            // Skip middleware for certain paths that should always be accessible
            var path = context.Request.Path.Value?.ToLower();
            if (IsExemptPath(path))
            {
                await _next(context);
                return;
            }

            // Check if user's email is confirmed
            var user = await userManager.GetUserAsync(context.User);
            if (user != null && !await userManager.IsEmailConfirmedAsync(user))
            {
                // Sign out the user and redirect to email confirmation
                await signInManager.SignOutAsync();
                context.Response.Redirect($"/Identity/Account/ResendEmailConfirmation?email={user.Email}");
                return;
            }

            await _next(context);
        }

        private static bool IsExemptPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            // Paths that unconfirmed users should be able to access
            var exemptPaths = new[]
            {
                "/identity/account/",
                "/account/",
                "/home/error",
                "/home/privacy",
                "/.well-known/",
                "/css/",
                "/js/",
                "/lib/",
                "/images/",
                "/favicon.ico"
            };

            return exemptPaths.Any(exemptPath => path.StartsWith(exemptPath));
        }
    }
}