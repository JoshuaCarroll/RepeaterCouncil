using Microsoft.AspNetCore.Identity;

namespace RepeaterCouncil.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Callsign { get; set; }
        public string FullName { get; set; }
    }

}
