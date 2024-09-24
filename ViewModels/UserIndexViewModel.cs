using RepeaterCouncil.Models;

namespace RepeaterCouncil.ViewModels
{
    public class UserIndexViewModel
    {
        public List<User> Users { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public string SearchTerm { get; set; }  // Store the search term for re-rendering the form

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }

}
