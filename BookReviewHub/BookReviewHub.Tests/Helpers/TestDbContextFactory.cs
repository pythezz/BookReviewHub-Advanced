using BookReviewHub.Data;
using BookReviewHub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookReviewHub.Tests.Helpers
{
    public static class TestDbContextFactory
    {
        public static ApplicationDbContext Create(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            return new ApplicationDbContext(options);
        }

        public static ApplicationDbContext CreateWithSeed(string dbName)
        {
            var context = Create(dbName);
            SeedTestData(context);
            return context;
        }

        private static void SeedTestData(ApplicationDbContext context)
        {
            context.Genres.AddRange(
                new Genre { Id = 1, Name = "Fiction" },
                new Genre { Id = 2, Name = "Science Fiction" }
            );

            context.Authors.AddRange(
                new Author { Id = 1, Name = "George Orwell", Nationality = "British", BirthYear = 1903 },
                new Author { Id = 2, Name = "Frank Herbert", Nationality = "American", BirthYear = 1920 }
            );

            context.SaveChanges();

            context.Books.AddRange(
                new Book
                {
                    Id = 1,
                    Title = "1984",
                    AuthorId = 1,
                    GenreId = 2,
                    PublicationYear = 1949,
                    Description = "A dystopian novel about totalitarianism."
                },
                new Book
                {
                    Id = 2,
                    Title = "Dune",
                    AuthorId = 2,
                    GenreId = 2,
                    PublicationYear = 1965,
                    Description = "An epic sci-fi saga."
                },
                new Book
                {
                    Id = 3,
                    Title = "Animal Farm",
                    AuthorId = 1,
                    GenreId = 1,
                    PublicationYear = 1945,
                    Description = "An allegorical novella."
                }
            );

            var hasher = new PasswordHasher<ApplicationUser>();
            var testUser = new ApplicationUser
            {
                Id = "test-user-id-1",
                UserName = "test@test.com",
                Email = "test@test.com",
                DisplayName = "TestUser",
                RegisteredAt = DateTime.UtcNow,
                SecurityStamp = "test-security-stamp"
            };
            testUser.PasswordHash = hasher.HashPassword(testUser, "Test@123");
            context.Users.Add(testUser);

            context.SaveChanges();

            context.Reviews.AddRange(
                new Review
                {
                    Id = 1,
                    BookId = 1,
                    UserId = "test-user-id-1",
                    Content = "A masterpiece of dystopian fiction.",
                    Rating = 5,
                    CreatedAt = DateTime.UtcNow.AddDays(-10)
                },
                new Review
                {
                    Id = 2,
                    BookId = 2,
                    UserId = "test-user-id-1",
                    Content = "Dense but rewarding world-building.",
                    Rating = 4,
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                }
            );

            context.SaveChanges();
        }
    }
}