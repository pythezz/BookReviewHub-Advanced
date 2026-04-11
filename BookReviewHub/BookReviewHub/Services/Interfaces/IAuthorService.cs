using BookReviewHub.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookReviewHub.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorViewModel>> GetAllAsync();
        Task<AuthorViewModel?> GetByIdAsync(int id);
        Task<AuthorFormModel?> GetFormModelByIdAsync(int id);
        Task CreateAsync(AuthorFormModel model);
        Task<bool> UpdateAsync(AuthorFormModel model);
        Task<bool> DeleteAsync(int id);
        Task<bool> HasBooksAsync(int id);
        Task<IEnumerable<SelectListItem>> GetSelectListAsync();
    }
}