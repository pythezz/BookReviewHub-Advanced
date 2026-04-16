using BookReviewHub.Data;
using BookReviewHub.Helpers;
using BookReviewHub.Models;
using BookReviewHub.Services.Interfaces;
using BookReviewHub.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public async Task<PaginatedList<BookViewModel>> GetPagedAsync(
            string? search, int? genreId, string? sortBy, int page, int pageSize)
        {
            var query = _context.Books
                .Include(b => b.Genre)
                .Include(b => b.Author)
                .Include(b => b.Reviews)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                query = query.Where(b =>
                    b.Title.ToLower().Contains(term) ||
                    b.Author.Name.ToLower().Contains(term));
            }

            if (genreId.HasValue)
                query = query.Where(b => b.GenreId == genreId.Value);

            query = sortBy switch
            {
                "year_desc" => query.OrderByDescending(b => b.PublicationYear),
                "year_asc" => query.OrderBy(b => b.PublicationYear),
                "rating" => query.OrderByDescending(b =>
                                   b.Reviews.Any()
                                       ? b.Reviews.Average(r => (double)r.Rating)
                                       : 0),
                _ => query.OrderBy(b => b.Title)
            };

            var projected = query.Select(b => new BookViewModel
            {
                Id = b.Id,
                Title = b.Title,
                AuthorName = b.Author != null ? b.Author.Name : "Unknown",
                Description = b.Description ?? string.Empty,
                PublicationYear = b.PublicationYear,
                GenreName = b.Genre != null ? b.Genre.Name : "Unknown",
                AverageRating = b.Reviews.Any()
                                      ? Math.Round(b.Reviews.Average(r => (double)r.Rating), 1)
                                      : 0,
                ReviewCount = b.Reviews.Count
            });

            return await PaginatedList<BookViewModel>.CreateAsync(projected, page, pageSize);
        }

        public async Task<BookViewModel?> GetByIdAsync(int id)
        {
            var book = await _context.Books
                .Include(b => b.Genre)
                .Include(b => b.Author)
                .Include(b => b.Reviews)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return null;

            return new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                AuthorName = book.Author?.Name ?? "Unknown",
                Description = book.Description ?? string.Empty,
                PublicationYear = book.PublicationYear,
                GenreName = book.Genre?.Name ?? "Unknown",
                AverageRating = book.Reviews.Any()
                                      ? Math.Round(book.Reviews.Average(r => (double)r.Rating), 1)
                                      : 0,
                ReviewCount = book.Reviews.Count
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
                Description = book.Description ?? string.Empty,
                PublicationYear = book.PublicationYear,
                GenreId = book.GenreId,
                AuthorId = book.AuthorId
            };
        }

        public async Task CreateAsync(BookFormModel model)
        {
            var book = new Book
            {
                Title = model.Title,
                Description = model.Description,
                PublicationYear = model.PublicationYear,
                GenreId = model.GenreId,
                AuthorId = model.AuthorId
            };
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(BookFormModel model)
        {
            var book = await _context.Books.FindAsync(model.Id);
            if (book == null) return false;

            book.Title = model.Title;
            book.Description = model.Description;
            book.PublicationYear = model.PublicationYear;
            book.GenreId = model.GenreId;
            book.AuthorId = model.AuthorId;

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
            => await _context.Books.AnyAsync(b => b.Id == id);

        public async Task<IEnumerable<SelectListItem>> GetSelectListAsync()
        {
            return await _context.Books
                .OrderBy(b => b.Title)
                .Select(b => new SelectListItem
                {
                    Value = b.Id.ToString(),
                    Text = b.Title
                })
                .ToListAsync();
        }
    }
}