using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookReviewHub.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Title { get; set; } = null!;

        [StringLength(2000)]
        public string? Description { get; set; }

        [Range(1000, 2100)]
        public int PublicationYear { get; set; }

        [StringLength(255)]
        public string? CoverImageUrl { get; set; }

        [Required]
        public int GenreId { get; set; }

        [ForeignKey(nameof(GenreId))]
        public Genre? Genre { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public Author? Author { get; set; }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<ReadingListItem> ReadingListItems { get; set; } = new List<ReadingListItem>();

    }
}