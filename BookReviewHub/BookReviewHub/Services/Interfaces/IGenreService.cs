using BookReviewHub.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookReviewHub.Services.Interfaces
{
    public interface IGenreService
    {
        Task<IEnumerable<GenreViewModel>> GetAllAsync();
        Task<GenreViewModel?> GetByIdAsync(int id);
        Task<GenreFormModel?> GetFormModelByIdAsync(int id);
        Task CreateAsync(GenreFormModel model);
        Task<bool> UpdateAsync(GenreFormModel model);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> HasBooksAsync(int id);
        Task<IEnumerable<SelectListItem>> GetSelectListAsync();
    }
}
