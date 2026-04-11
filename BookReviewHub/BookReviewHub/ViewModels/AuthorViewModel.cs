namespace BookReviewHub.ViewModels
{
    public class AuthorViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Nationality { get; set; }
        public int? BirthYear { get; set; }
        public string? Biography { get; set; }
        public int BookCount { get; set; }
    }
}