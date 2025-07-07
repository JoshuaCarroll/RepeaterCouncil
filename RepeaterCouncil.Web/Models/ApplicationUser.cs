using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace RepeaterCouncil.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Callsign")]
        public string Callsign { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }
    }

}
