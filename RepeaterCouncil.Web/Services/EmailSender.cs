using SendGrid.Helpers.Mail;
using SendGrid;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.IdentityModel.Tokens;
using RepeaterCouncil.Web.Models;

namespace RepeaterCouncil.Web.Services
{
    public interface IEmailSender : Microsoft.AspNetCore.Identity.UI.Services.IEmailSender
    {
        new Task SendEmailAsync(string email, string subject, string htmlMessage);
        Task SendEmailAsync(string[] emails, string subject, string htmlMessage);
        Task SendEmailAsync(List<EmailAddress> to, string subject, string htmlMessage);
        Task SendEmailAsync(SendGridMessage sendGridMessage);
        Task SendTemplateEmailAsync(List<string> emails, string subject, string templateId, object dynamicTemplateData);
        Task<string> SendTemplateEmailAsync<TModel>(string email, string templateName, TModel model);
    }

    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly ISimpleEmailTemplateService _emailTemplateService;
        private EmailAddress fromEmailAddress;

        public EmailSender(IConfiguration configuration, ISimpleEmailTemplateService emailTemplateService)
        {
            _configuration = configuration;
            _emailTemplateService = emailTemplateService;

            string? _sendGridApiKey = _configuration["SendGridApiKey"];
            string? _sendGridSenderEmail = _configuration["SendGridSenderEmail"];
            string? _sendGridSenderName = _configuration["SendGridSenderName"];

            if (string.IsNullOrEmpty(_sendGridApiKey) || string.IsNullOrEmpty(_sendGridSenderEmail) || string.IsNullOrEmpty(_sendGridSenderName))
            {
                throw new InvalidOperationException("SendGrid configuration is not properly set in the app settings.");
            }

            fromEmailAddress = new EmailAddress(_configuration["SendGridSenderEmail"], _configuration["SendGridSenderName"]);
        }

        public async Task SendTemplateEmailAsync(List<string> toEmailAddresses, string subject, string templateId, object dynamicTemplateData)
        {
            var to = toEmailAddresses.Select(email => new EmailAddress(email)).ToList();

            SendGridMessage message = new SendGridMessage();
            message.AddCustomArg("application", "RepeaterCouncil.org");
            message.TemplateId = templateId;
            message.SetTemplateData(dynamicTemplateData);
            message.From = fromEmailAddress;
            message.Subject = subject;
            message.AddTos(to);

            var apiKey = _configuration["SendGridApiKey"];
            var client = new SendGridClient(apiKey);
            var response = await client.SendEmailAsync(message);
            var responseBody = await response.Body.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                // TODO: Perhaps tell someone?
            }
        }

        public async Task SendEmailAsync(SendGridMessage message)
        {
            var apiKey = _configuration["SendGridApiKey"];
            var client = new SendGridClient(apiKey);

            message.AddCustomArg("application", "RepeaterCouncil.org");

            var response = await client.SendEmailAsync(message);
            if (!response.IsSuccessStatusCode)
            {
                // TODO: Log this error and notify someone
            }
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var from = fromEmailAddress;
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, string.Empty, htmlMessage);
            await SendEmailAsync(msg);
        }

        public async Task SendEmailAsync(string[] emails, string subject, string htmlMessage)
        {
            var to = emails
                .Select(email => new EmailAddress(email))
                .ToList();

            await SendEmailAsync(to, subject, htmlMessage);
        }

        public async Task SendEmailAsync(List<EmailAddress> to, string subject, string htmlMessage)
        {
            var from = fromEmailAddress;
            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, to, subject, string.Empty, htmlMessage);
            await SendEmailAsync(msg);
        }

        public async Task<string> SendTemplateEmailAsync<TModel>(string email, string templateName, TModel model)
        {
            var htmlContent = await _emailTemplateService.RenderTemplateAsync(templateName, model);

            // Extract subject from the model if it has a Subject property
            var subject = "Email from Repeater Council";
            if (model is EmailConfirmationViewModel emailModel)
            {
                subject = emailModel.Subject;
            }
            else if (model is PasswordResetViewModel passwordResetModel)
            {
                subject = passwordResetModel.Subject;
            }
            else if (model is WelcomeEmailViewModel welcomeModel)
            {
                subject = welcomeModel.Subject;
            }
            else if (model is AnnouncementEmailViewModel announcementModel)
            {
                subject = announcementModel.Subject;
            }

            await SendEmailAsync(email, subject, htmlContent);
            return htmlContent;
        }
    }
}
