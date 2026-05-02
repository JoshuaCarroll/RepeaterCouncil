// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using RepeaterCouncil.Web.Models;
using RepeaterCouncil.Web.Services;

namespace RepeaterCouncil.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RepeaterCouncil.Web.Services.IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<ApplicationUser> userManager, RepeaterCouncil.Web.Services.IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Debug: Check if method is being called
            System.Diagnostics.Debug.WriteLine($"ForgotPassword OnPostAsync called for email: {Input?.Email}");

            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("ModelState is valid");
                var user = await _userManager.FindByEmailAsync(Input.Email);
                System.Diagnostics.Debug.WriteLine($"User found: {user != null}, Email confirmed: {user != null && await _userManager.IsEmailConfirmedAsync(user)}");

                if (user == null)
                {
                    System.Diagnostics.Debug.WriteLine("User not found - redirecting without sending email");
                    // Don't reveal that the user does not exist
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                if (!(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    System.Diagnostics.Debug.WriteLine("Email not confirmed - redirecting to resend confirmation");
                    // Redirect to email confirmation instead of generic confirmation
                    return RedirectToPage("./ResendEmailConfirmation", new { email = Input.Email });
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                var tenantName = HttpContext.Items["TenantName"]?.ToString() ?? "Repeater Council";

                var emailModel = new PasswordResetViewModel
                {
                    TenantName = tenantName,
                    FullName = user.FullName,
                    Email = Input.Email,
                    CallbackUrl = callbackUrl,
                    Subject = $"Reset your {tenantName} password"
                };

                System.Diagnostics.Debug.WriteLine("About to send password reset email");
                await _emailSender.SendTemplateEmailAsync(Input.Email, "PasswordReset", emailModel);
                System.Diagnostics.Debug.WriteLine("Password reset email sent successfully");

                return RedirectToPage("./ForgotPasswordConfirmation");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("ModelState is invalid");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    System.Diagnostics.Debug.WriteLine($"ModelState error: {error.ErrorMessage}");
                }
            }

            return Page();
        }
    }
}