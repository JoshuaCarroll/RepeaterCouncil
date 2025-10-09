using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace RepeaterCouncil.Web.Services
{
    public interface IEmailTemplateService
    {
        Task<string> RenderTemplateAsync<TModel>(string templateName, TModel model);
    }

    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly IServiceProvider _serviceProvider;
        private readonly ITempDataProvider _tempDataProvider;

        public EmailTemplateService(
            IRazorViewEngine razorViewEngine,
            IServiceProvider serviceProvider,
            ITempDataProvider tempDataProvider)
        {
            _razorViewEngine = razorViewEngine;
            _serviceProvider = serviceProvider;
            _tempDataProvider = tempDataProvider;
        }

        public async Task<string> RenderTemplateAsync<TModel>(string templateName, TModel model)
        {
            System.Diagnostics.Debug.WriteLine($"EmailTemplateService: Attempting to render template '{templateName}'");

            var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            // Try with just the template name since we have a view location expander
            System.Diagnostics.Debug.WriteLine($"EmailTemplateService: Searching for view '{templateName}'");
            var viewResult = _razorViewEngine.FindView(actionContext, templateName, false);

            if (!viewResult.Success)
            {
                var searchedLocations = viewResult.SearchedLocations != null
                    ? string.Join(", ", viewResult.SearchedLocations)
                    : "No locations searched";
                System.Diagnostics.Debug.WriteLine($"EmailTemplateService: View not found. Searched locations: {searchedLocations}");
                throw new ArgumentException($"A view with the name '{templateName}' could not be found. Searched locations: {searchedLocations}");
            }

            System.Diagnostics.Debug.WriteLine($"EmailTemplateService: View '{templateName}' found successfully");

            var viewData = new ViewDataDictionary<TModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            };

            var tempData = new TempDataDictionary(actionContext.HttpContext, _tempDataProvider);

            using var stringWriter = new StringWriter();
            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                viewData,
                tempData,
                stringWriter,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);
            return stringWriter.ToString();
        }
    }
}