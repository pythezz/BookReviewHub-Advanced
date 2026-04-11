using BookReviewHub.Models;
using Microsoft.EntityFrameworkCore;

namespace BookReviewHub.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Author> Authors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Genre)
                .WithMany(g => g.Books)
                .HasForeignKey(b => b.GenreId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Fiction" },
                new Genre { Id = 2, Name = "Non-Fiction" },
                new Genre { Id = 3, Name = "Science Fiction" },
                new Genre { Id = 4, Name = "Fantasy" },
                new Genre { Id = 5, Name = "Mystery" },
                new Genre { Id = 6, Name = "History" },
                new Genre { Id = 7, Name = "Biography" },
                new Genre { Id = 8, Name = "Self-Help" },
                new Genre { Id = 9, Name = "Romance" },
                new Genre { Id = 10, Name = "Thriller" }
            );

            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, Name = "George Orwell", Nationality = "British", BirthYear = 1903, Biography = "Eric Arthur Blair, known by his pen name George Orwell, was an English novelist and essayist." },
                new Author { Id = 2, Name = "J.K. Rowling", Nationality = "British", BirthYear = 1965, Biography = "Author of the Harry Potter fantasy series." },
                new Author { Id = 3, Name = "Frank Herbert", Nationality = "American", BirthYear = 1920, Biography = "Best known for the Dune series of science fiction novels." },
                new Author { Id = 4, Name = "Agatha Christie", Nationality = "British", BirthYear = 1890, Biography = "Known for her 66 detective novels and 14 short story collections." },
                new Author { Id = 5, Name = "J.R.R. Tolkien", Nationality = "British", BirthYear = 1892, Biography = "Author of The Hobbit and The Lord of the Rings." }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "1984", AuthorId = 1, GenreId = 3, PublicationYear = 1949, Description = "A dystopian novel set in a totalitarian society under constant surveillance." },
                new Book { Id = 2, Title = "Animal Farm", AuthorId = 1, GenreId = 1, PublicationYear = 1945, Description = "An allegorical novella about farm animals who rebel against their farmer." },
                new Book { Id = 3, Title = "Harry Potter and the Philosopher's Stone", AuthorId = 2, GenreId = 4, PublicationYear = 1997, Description = "A young boy discovers he is a wizard and attends Hogwarts School." },
                new Book { Id = 4, Title = "Dune", AuthorId = 3, GenreId = 3, PublicationYear = 1965, Description = "An epic sci-fi saga set on the desert planet Arrakis." },
                new Book { Id = 5, Title = "Murder on the Orient Express", AuthorId = 4, GenreId = 5, PublicationYear = 1934, Description = "Hercule Poirot investigates a murder aboard a luxury train." },
                new Book { Id = 6, Title = "The Fellowship of the Ring", AuthorId = 5, GenreId = 4, PublicationYear = 1954, Description = "Frodo Baggins begins his quest to destroy the One Ring." }
            );
        }
    }
}