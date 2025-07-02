using Microsoft.AspNetCore.Identity;

namespace RepeaterCouncil.Web.Models
{
    public class RepeaterNote
    {
        public int Id { get; set; }

        public int RepeaterId { get; set; }
        public Repeater Repeater { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Note { get; set; }
    }

}
