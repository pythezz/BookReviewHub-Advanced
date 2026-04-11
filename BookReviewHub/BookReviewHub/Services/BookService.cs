using BookReviewHub.Data;
using BookReviewHub.Models;
using BookReviewHub.Services.Interfaces;
using BookReviewHub.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BookReviewHub.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;

        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookViewModel>> GetAllAsync()
        {
            return await _context.Books
                .Include(b => b.Genre)
                .Select(b => new BookViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Description = b.Description ?? string.Empty,
                    PublicationYear = b.PublicationYear,
                    Rating = b.Rating,
                    GenreName = b.Genre != null ? b.Genre.Name : "Unknown"
                })
                .ToListAsync();
        }

        public async Task<BookViewModel?> GetByIdAsync(int id)
        {
            var book = await _context.Books
                .Include(b => b.Genre)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return null;

            return new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description ?? string.Empty,
                PublicationYear = book.PublicationYear,
                Rating = book.Rating,
                GenreName = book.Genre?.Name ?? "Unknown"
            };
        }

        public async Task<BookFormModel?> GetFormModelByIdAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null) return null;

            return new BookFormModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description ?? string.Empty,
                PublicationYear = book.PublicationYear,
                Rating = book.Rating,
                GenreId = book.GenreId
            };
        }

        public async Task CreateAsync(BookFormModel model)
        {
            var book = new Book
            {
                Title = model.Title,
                Author = model.Author,
                Description = model.Description,
                PublicationYear = model.PublicationYear,
                Rating = model.Rating,
                GenreId = model.GenreId
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(BookFormModel model)
        {
            var book = await _context.Books.FindAsync(model.Id);

            if (book == null) return false;

            book.Title = model.Title;
            book.Author = model.Author;
            book.Description = model.Description;
            book.PublicationYear = model.PublicationYear;
            book.Rating = model.Rating;
            book.GenreId = model.GenreId;

            _context.Books.Update(book);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null) return false;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Books.AnyAsync(b => b.Id == id);
        }
    }
}
