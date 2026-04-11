using System.ComponentModel.DataAnnotations;

namespace BookReviewHub.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        [StringLength(1000)]
        public string? Biography { get; set; }

        [StringLength(100)]
        public string? Nationality { get; set; }

        public int? BirthYear { get; set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}