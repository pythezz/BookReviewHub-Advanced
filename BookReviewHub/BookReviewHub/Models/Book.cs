using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookReviewHub.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Author { get; set; } = null!;

        [Required]
        [StringLength(2000)]
        public string? Description { get; set; }

        [Range(0, 2026)]
        public int PublicationYear { get; set; }

        [Range(1, 5)]
        public double Rating { get; set; }

        [Required]
        public int GenreId { get; set; }

        [ForeignKey(nameof(GenreId))]
        public Genre? Genre { get; set; }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
