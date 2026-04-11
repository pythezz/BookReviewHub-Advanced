namespace BookReviewHub.ViewModels
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int PublicationYear { get; set; }
        public double Rating { get; set; }
        public string GenreName { get; set; } = null!;
    }
}
