using RepeaterCouncil.Web.Enums;
using System.ComponentModel.DataAnnotations;

namespace RepeaterCouncil.Web.Models
{
    public class Link
    {
        public int Id { get; set; }

        [Display(Name = "Repeater ID")]
        public int RepeaterId { get; set; }

        public Repeater Repeater { get; set; }

        [Display(Name = "Link Type")]
        [Required(ErrorMessage = "Link Type is required.")]
        public LinkType LinkType { get; set; } // from code table or enum

        public string LinkDetails { get; set; } = string.Empty;

        public int? LinkedRepeaterId { get; set; }
        [Display(Name = "Linked Repeater")]
        public Repeater? LinkedRepeater { get; set; }
    }
}
