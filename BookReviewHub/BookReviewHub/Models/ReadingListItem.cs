using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookReviewHub.Models
{
    public enum ReadingStatus
    {
        WantToRead,
        CurrentlyReading,
        Read
    }

    public class ReadingListItem
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        [Required]
        public int BookId { get; set; }

        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; } = null!;

        public ReadingStatus Status { get; set; } = ReadingStatus.WantToRead;

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}