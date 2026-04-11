using BookReviewHub.ViewModels;

namespace BookReviewHub.Services.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookViewModel>> GetAllAsync();
        Task<BookViewModel?> GetByIdAsync(int id);
        Task<BookFormModel?> GetFormModelByIdAsync(int id);
        Task CreateAsync(BookFormModel model);
        Task<bool> UpdateAsync(BookFormModel model);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
