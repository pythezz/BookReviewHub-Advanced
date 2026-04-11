using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookReviewHub.ViewModels
{
    public class BookFormModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 100 characters.")]
        [Display(Name = "Title")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Author is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Author name must be between 2 and 100 characters.")]
        [Display(Name = "Author")]
        public string Author { get; set; } = null!;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 2000 characters.")]
        [Display(Name = "Description")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Publication year is required.")]
        [Range(1000, 2026, ErrorMessage = "Publication year must be between 1000 and 2026.")]
        [Display(Name = "Publication Year")]
        public int PublicationYear { get; set; }

        [Required(ErrorMessage = "Rating is required.")]
        [Range(1.0, 5.0, ErrorMessage = "Rating must be between 1 and 5.")]
        [Display(Name = "Rating")]
        public double Rating { get; set; }

        [Required(ErrorMessage = "Please select a genre.")]
        [Display(Name = "Genre")]
        public int GenreId { get; set; }

        public IEnumerable<SelectListItem> Genres { get; set; } = new List<SelectListItem>();
    }
}
