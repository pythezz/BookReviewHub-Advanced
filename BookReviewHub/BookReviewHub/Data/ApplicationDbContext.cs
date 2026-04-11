using BookReviewHub.Models;
using Microsoft.EntityFrameworkCore;

namespace BookReviewHub.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Review> Reviews { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Genre>()
                .HasData(
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

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Genre)
                .WithMany(g => g.Books)
                .HasForeignKey(b => b.GenreId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
