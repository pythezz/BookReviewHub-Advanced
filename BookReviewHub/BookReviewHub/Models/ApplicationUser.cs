using Microsoft.AspNetCore.Identity;

namespace BookReviewHub.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string DisplayName { get; set; } = string.Empty;

        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<ReadingListItem> ReadingList { get; set; } = new List<ReadingListItem>();

    }
}