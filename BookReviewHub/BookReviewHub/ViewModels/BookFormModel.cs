using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookReviewHub.ViewModels
{
    public class BookFormModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters.")]
        [Display(Name = "Title")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 2000 characters.")]
        [Display(Name = "Description")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Publication year is required.")]
        [Range(1000, 2100, ErrorMessage = "Publication year must be between 1000 and 2100.")]
        [Display(Name = "Publication Year")]
        public int PublicationYear { get; set; }

        [Required(ErrorMessage = "Please select a genre.")]
        [Display(Name = "Genre")]
        public int GenreId { get; set; }

        [Required(ErrorMessage = "Please select an author.")]
        [Display(Name = "Author")]
        public int AuthorId { get; set; }

        public IEnumerable<SelectListItem> Genres { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Authors { get; set; } = new List<SelectListItem>();
    }
}