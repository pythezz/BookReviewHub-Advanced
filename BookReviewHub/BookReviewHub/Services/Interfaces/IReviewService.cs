using BookReviewHub.ViewModels;

namespace BookReviewHub.Services.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewViewModel>> GetByBookAsync(int bookId);
        Task<IEnumerable<ReviewViewModel>> GetByUserAsync(string userId);
        Task<ReviewViewModel?> GetByIdAsync(int id);
        Task<ReviewFormModel?> GetFormModelByIdAsync(int id);
        Task<int> CreateAsync(ReviewFormModel model, string userId);
        Task<bool> UpdateAsync(ReviewFormModel model, string userId);
        Task<bool> DeleteAsync(int id, string userId, bool isAdmin);
        Task<bool> UserHasReviewedBookAsync(int bookId, string userId);
    }
}