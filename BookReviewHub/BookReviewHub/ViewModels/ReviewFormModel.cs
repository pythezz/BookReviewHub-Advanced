using System.ComponentModel.DataAnnotations;

namespace BookReviewHub.ViewModels
{
    public class ReviewFormModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Review content is required.")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Review must be between 10 and 1000 characters.")]
        [Display(Name = "Your Review")]
        public string Content { get; set; } = null!;

        [Required(ErrorMessage = "Rating is required.")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        [Display(Name = "Rating (1-5)")]
        public int Rating { get; set; }

        [Required]
        public int BookId { get; set; }

        public string BookTitle { get; set; } = string.Empty;
    }
}