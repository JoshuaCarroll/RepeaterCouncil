// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Linq;
using System.Text;
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
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RepeaterCouncil.Web.Services.IEmailSender _emailSender;

        public ConfirmEmailModel(UserManager<ApplicationUser> userManager, RepeaterCouncil.Web.Services.IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }
        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                StatusMessage = "Thank you for confirming your email.";

                // Send welcome email after successful confirmation
                try
                {
                    var tenantName = HttpContext.Items["TenantName"]?.ToString() ?? "Repeater Council";
                    var loginUrl = Url.Page("/Account/Login", null, null, Request.Scheme);

                    var welcomeModel = new WelcomeEmailViewModel
                    {
                        TenantName = tenantName,
                        FullName = user.FullName,
                        Callsign = user.Callsign,
                        Email = user.Email,
                        LoginUrl = loginUrl,
                        ActivationDate = DateTime.Now,
                        Subject = $"Welcome to {tenantName}!"
                    };

                    await _emailSender.SendTemplateEmailAsync(user.Email, "Welcome", welcomeModel);
                }
                catch
                {
                    // Don't fail the confirmation if welcome email fails
                }
            }
            else
            {
                StatusMessage = "Error confirming your email.";
            }

            return Page();
        }
    }
}