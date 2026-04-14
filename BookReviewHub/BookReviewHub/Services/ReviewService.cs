using BookReviewHub.Data;
using BookReviewHub.Models;
using BookReviewHub.Services.Interfaces;
using BookReviewHub.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BookReviewHub.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;

        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReviewViewModel>> GetByBookAsync(int bookId)
        {
            return await _context.Reviews
                .Where(r => r.BookId == bookId)
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new ReviewViewModel
                {
                    Id = r.Id,
                    Content = r.Content,
                    Rating = r.Rating,
                    CreatedAt = r.CreatedAt,
                    UserDisplayName = r.User.DisplayName,
                    UserId = r.UserId,
                    BookId = r.BookId,
                    BookTitle = r.Book.Title
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ReviewViewModel>> GetByUserAsync(string userId)
        {
            return await _context.Reviews
                .Where(r => r.UserId == userId)
                .Include(r => r.Book)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new ReviewViewModel
                {
                    Id = r.Id,
                    Content = r.Content,
                    Rating = r.Rating,
                    CreatedAt = r.CreatedAt,
                    UserDisplayName = r.User.DisplayName,
                    UserId = r.UserId,
                    BookId = r.BookId,
                    BookTitle = r.Book.Title
                })
                .ToListAsync();
        }

        public async Task<ReviewViewModel?> GetByIdAsync(int id)
        {
            return await _context.Reviews
                .Where(r => r.Id == id)
                .Include(r => r.User)
                .Include(r => r.Book)
                .Select(r => new ReviewViewModel
                {
                    Id = r.Id,
                    Content = r.Content,
                    Rating = r.Rating,
                    CreatedAt = r.CreatedAt,
                    UserDisplayName = r.User.DisplayName,
                    UserId = r.UserId,
                    BookId = r.BookId,
                    BookTitle = r.Book.Title
                })
                .FirstOrDefaultAsync();
        }

        public async Task<ReviewFormModel?> GetFormModelByIdAsync(int id)
        {
            var review = await _context.Reviews
                .Include(r => r.Book)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (review == null) return null;

            return new ReviewFormModel
            {
                Id = review.Id,
                Content = review.Content,
                Rating = review.Rating,
                BookId = review.BookId,
                BookTitle = review.Book.Title
            };
        }

        public async Task<int> CreateAsync(ReviewFormModel model, string userId)
        {
            var review = new Review
            {
                Content = model.Content,
                Rating = model.Rating,
                BookId = model.BookId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return review.Id;
        }

        public async Task<bool> UpdateAsync(ReviewFormModel model, string userId)
        {
            var review = await _context.Reviews.FindAsync(model.Id);
            if (review == null) return false;
            if (review.UserId != userId) return false;

            review.Content = model.Content;
            review.Rating = model.Rating;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id, string userId, bool isAdmin)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return false;
            if (!isAdmin && review.UserId != userId) return false;

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UserHasReviewedBookAsync(int bookId, string userId)
            => await _context.Reviews.AnyAsync(r => r.BookId == bookId && r.UserId == userId);
    }
}