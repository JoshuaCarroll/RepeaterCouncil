using RepeaterCouncil.Web.Enums;
using System.ComponentModel.DataAnnotations;

namespace RepeaterCouncil.Web.Models
{
    public class Link
    {
        public int Id { get; set; }

        [Display(Name = "Repeater ID")]
        public int RepeaterId { get; set; }

        public Repeater? Repeater { get; set; } = null;

        [Display(Name = "Link Type")]
        [Required(ErrorMessage = "Link Type is required.")]
        public LinkType LinkType { get; set; }

        [Display(Name = "Link Details", Description = "Details a reasonable operator would need to use this connection")]
        public string LinkDetails { get; set; } = string.Empty;

        public int? LinkedRepeaterId { get; set; }
        [Display(Name = "Linked Repeater")]
        public Repeater? LinkedRepeater { get; set; } = null;
    }
}
