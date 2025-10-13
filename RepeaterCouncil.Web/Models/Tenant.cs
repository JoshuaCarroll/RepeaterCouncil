using Microsoft.AspNetCore.Mvc.ModelBinding;
using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace RepeaterCouncil.Web.Models
{
    public class Tenant
    {
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        [RegularExpression(@"^[-a-zA-Z0-9@:%._\+~#=]{0,256}\.?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,19}$",
            ErrorMessage = "URL must be a valid URL part or domain (e.g. arkansasrepeatercouncil.org or al.repeatercouncil.org)")]
        public required string Url { get; set; }

        [Display(Name = "About Us Content")]
        public string? AboutUsContent { get; set; }

        [Display(Name = "Geographic Borders")]
        public Geometry? Borders { get; set; }

        [BindNever]
        public ICollection<Repeater> Repeaters { get; set; } = new List<Repeater>();

        [BindNever]
        public ICollection<CodeTable> CodeTables { get; set; } = new List<CodeTable>();
    }

}
