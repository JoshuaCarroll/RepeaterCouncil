namespace RepeaterCouncil.Web.Models
{
    public class Tenant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; } // i.e. "arkansas"
        public ICollection<Repeater> Repeaters { get; set; }
        public ICollection<CodeTable> CodeTables { get; set; }
    }

}
