namespace BookReviewHub.ViewModels
{
    public class ReviewViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserDisplayName { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
    }
}