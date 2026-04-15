using BookReviewHub.Data;
using BookReviewHub.Models;
using BookReviewHub.Services.Interfaces;
using BookReviewHub.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BookReviewHub.Services
{
    public class ReadingListService : IReadingListService
    {
        private readonly ApplicationDbContext _context;

        public ReadingListService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReadingListItemViewModel>> GetByUserAsync(string userId)
        {
            return await _context.ReadingListItems
                .Where(rl => rl.UserId == userId)
                .Include(rl => rl.Book)
                    .ThenInclude(b => b.Author)
                .OrderByDescending(rl => rl.AddedAt)
                .Select(rl => new ReadingListItemViewModel
                {
                    Id = rl.Id,
                    BookId = rl.BookId,
                    BookTitle = rl.Book.Title,
                    AuthorName = rl.Book.Author != null ? rl.Book.Author.Name : "Unknown",
                    Status = rl.Status.ToString(),
                    AddedAt = rl.AddedAt
                })
                .ToListAsync();
        }

        public async Task<bool> AddAsync(int bookId, string userId)
        {
            var exists = await _context.ReadingListItems
                .AnyAsync(rl => rl.BookId == bookId && rl.UserId == userId);

            if (exists) return false;

            _context.ReadingListItems.Add(new ReadingListItem
            {
                BookId = bookId,
                UserId = userId,
                Status = ReadingStatus.WantToRead,
                AddedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAsync(int bookId, string userId)
        {
            var item = await _context.ReadingListItems
                .FirstOrDefaultAsync(rl => rl.BookId == bookId && rl.UserId == userId);

            if (item == null) return false;

            _context.ReadingListItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateStatusAsync(int bookId, string userId, ReadingStatus status)
        {
            var item = await _context.ReadingListItems
                .FirstOrDefaultAsync(rl => rl.BookId == bookId && rl.UserId == userId);

            if (item == null) return false;

            item.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsInListAsync(int bookId, string userId)
            => await _context.ReadingListItems
                .AnyAsync(rl => rl.BookId == bookId && rl.UserId == userId);
    }
}