using SendGrid.Helpers.Mail;
using SendGrid;
using HtmlAgilityPack;

namespace RepeaterCouncil.Web.Services
{
    public interface IEmailSender : Microsoft.AspNetCore.Identity.UI.Services.IEmailSender
    {
        new Task SendEmailAsync(string email, string subject, string htmlMessage);
        Task SendEmailAsync(string[] emails, string subject, string htmlMessage);
        Task SendEmailAsync(List<EmailAddress> to, string subject, string htmlMessage);
        Task SendEmailAsync(SendGridMessage sendGridMessage);
        Task SendTemplateEmailAsync(List<string> emails, string subject, string templateId, object dynamicTemplateData);
    }

    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        //private readonly ILogger<EmailSender> _logger;
        private EmailAddress fromEmailAddress = new();

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
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
                // Perhaps tell someone?
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
                // Perhaps log the error or notify someone?
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
    }
}
