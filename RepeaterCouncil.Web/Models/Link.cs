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
        public string LinkType { get; set; } // from code table or enum

        public string Destination { get; set; }
        // - if LinkType is "RF" this might be another repeater callsign or id
        // - else could be AllStar node, EchoLink number, free-text
    }

}
