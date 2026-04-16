using BookReviewHub.Helpers;
using BookReviewHub.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookReviewHub.Services.Interfaces
{
    public interface IBookService
    {
        Task<PaginatedList<BookViewModel>> GetPagedAsync(
            string? search, int? genreId, string? sortBy, int page, int pageSize);
        Task<BookViewModel?> GetByIdAsync(int id);
        Task<BookFormModel?> GetFormModelByIdAsync(int id);
        Task CreateAsync(BookFormModel model);
        Task<bool> UpdateAsync(BookFormModel model);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<SelectListItem>> GetSelectListAsync();
    }
}