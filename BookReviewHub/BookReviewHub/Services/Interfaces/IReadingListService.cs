using BookReviewHub.Models;
using BookReviewHub.ViewModels;

namespace BookReviewHub.Services.Interfaces
{
    public interface IReadingListService
    {
        Task<IEnumerable<ReadingListItemViewModel>> GetByUserAsync(string userId);
        Task<bool> AddAsync(int bookId, string userId);
        Task<bool> RemoveAsync(int bookId, string userId);
        Task<bool> UpdateStatusAsync(int bookId, string userId, ReadingStatus status);
        Task<bool> IsInListAsync(int bookId, string userId);
    }
}