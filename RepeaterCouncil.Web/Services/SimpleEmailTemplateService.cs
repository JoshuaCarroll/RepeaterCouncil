using System.Reflection;
using System.Text.RegularExpressions;

namespace RepeaterCouncil.Web.Services
{
    public interface ISimpleEmailTemplateService
    {
        Task<string> RenderTemplateAsync<TModel>(string templateName, TModel model);
    }

    public class SimpleEmailTemplateService : ISimpleEmailTemplateService
    {
        private readonly IWebHostEnvironment _environment;

        public SimpleEmailTemplateService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> RenderTemplateAsync<TModel>(string templateName, TModel model)
        {
            var templatePath = Path.Combine(_environment.ContentRootPath, "EmailTemplates", $"{templateName}.cshtml");

            if (!File.Exists(templatePath))
            {
                throw new FileNotFoundException($"Email template '{templateName}' not found at path: {templatePath}");
            }

            var templateContent = await File.ReadAllTextAsync(templatePath);

            // Simple token replacement for @Model properties
            var result = await ProcessTemplate(templateContent, model);

            return result;
        }

        private async Task<string> ProcessTemplate<TModel>(string template, TModel model)
        {
            var result = template;

            if (model == null)
                return result;

            // Get all properties from the model
            var properties = typeof(TModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var value = property.GetValue(model)?.ToString() ?? string.Empty;

                // Replace @Model.PropertyName patterns
                var pattern = $@"@Model\.{property.Name}\b";
                result = Regex.Replace(result, pattern, value);
            }

            // Remove Razor directives that we can't process
            result = Regex.Replace(result, @"@\{[^}]*\}", string.Empty, RegexOptions.Multiline | RegexOptions.Singleline);
            result = Regex.Replace(result, @"@model[^\r\n]*", string.Empty);

            return result;
        }
    }
}