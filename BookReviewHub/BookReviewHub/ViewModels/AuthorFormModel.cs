using System.ComponentModel.DataAnnotations;

namespace BookReviewHub.ViewModels
{
    public class AuthorFormModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = null!;

        [StringLength(1000, ErrorMessage = "Biography cannot exceed 1000 characters.")]
        [Display(Name = "Biography")]
        public string? Biography { get; set; }

        [StringLength(100)]
        [Display(Name = "Nationality")]
        public string? Nationality { get; set; }

        [Range(1000, 2020, ErrorMessage = "Birth year must be between 1000 and 2020.")]
        [Display(Name = "Birth Year")]
        public int? BirthYear { get; set; }
    }
}