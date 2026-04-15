namespace BookReviewHub.ViewModels
{
    public class ReadingListItemViewModel
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime AddedAt { get; set; }
    }
}