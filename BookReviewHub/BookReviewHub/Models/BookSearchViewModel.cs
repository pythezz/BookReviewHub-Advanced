using BookReviewHub.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookReviewHub.ViewModels
{
    public class BookSearchViewModel
    {
        public string? SearchTerm { get; set; }
        public int? GenreId { get; set; }
        public string? SortBy { get; set; }
        public int CurrentPage { get; set; } = 1;
        public PaginatedList<BookViewModel>? Books { get; set; }
        public IEnumerable<SelectListItem> Genres { get; set; } = new List<SelectListItem>();
    }
}