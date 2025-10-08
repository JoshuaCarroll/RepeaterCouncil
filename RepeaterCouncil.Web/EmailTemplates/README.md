# Email Template System

This folder contains Razor templates for sending HTML emails from the RepeaterCouncil application.

## Available Templates

### EmailConfirmation.cshtml
Used for user registration email confirmation.
- **Model:** `EmailConfirmationViewModel`
- **Usage:** Registration process
- **Features:** Professional HTML design, tenant branding, confirmation button

### PasswordReset.cshtml
Used for password reset requests.
- **Model:** `PasswordResetViewModel`
- **Usage:** Password reset flow
- **Features:** Security warnings, reset button, expiration notice

### Welcome.cshtml
Used for welcoming new users after email confirmation.
- **Model:** `WelcomeEmailViewModel`
- **Usage:** Post-registration welcome
- **Features:** Account details, feature overview, login button

### Announcement.cshtml
General-purpose template for tenant announcements and notifications.
- **Model:** `AnnouncementEmailViewModel`
- **Usage:** Admin announcements, system notifications
- **Features:** Flexible content areas, optional action buttons, sender info

## Usage Example

```csharp
// In a controller or service
var emailModel = new EmailConfirmationViewModel
{
    TenantName = "Arkansas Repeater Council",
    FullName = "John Doe",
    Email = "john@example.com",
    CallbackUrl = "https://example.com/confirm",
    Subject = "Welcome - Please Confirm Your Email"
};

await _emailSender.SendTemplateEmailAsync(
    "john@example.com", 
    "EmailConfirmation", 
    emailModel
);
```

## Template Features

- **Responsive Design**: All templates use inline CSS for maximum email client compatibility
- **Tenant Branding**: Each template includes tenant-specific branding and colors
- **Professional Styling**: Bootstrap-inspired color scheme and typography
- **Accessibility**: Proper HTML structure and alt text for better accessibility
- **Security**: HTML encoding for all user-provided content

## Adding New Templates

1. Create a new `.cshtml` file in this folder
2. Create a corresponding view model in `Models/`
3. Use the existing templates as a reference for structure and styling
4. Register any new services in `Program.cs` if needed
5. Update this README with the new template information

## Technical Notes

- Templates are rendered using the `EmailTemplateService`
- All email sending goes through the `EmailSender` service with SendGrid integration
- Templates support full Razor syntax including conditionals and loops
- HTML is automatically encoded for security