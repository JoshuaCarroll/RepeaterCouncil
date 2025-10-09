using Microsoft.AspNetCore.Mvc.Razor;

namespace RepeaterCouncil.Web.Services
{
    public class EmailTemplateViewLocationExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            // Add EmailTemplates folder to the search locations
            var emailTemplateLocations = new[]
            {
                "/EmailTemplates/{0}.cshtml",
                "~/EmailTemplates/{0}.cshtml"
            };

            return emailTemplateLocations.Concat(viewLocations);
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            // No special values needed for this expander
        }
    }
}