using BookReviewHub.Services.Interfaces;
using BookReviewHub.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookReviewHub.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        public async Task<IActionResult> Index()
        {
            var authors = await _authorService.GetAllAsync();
            return View(authors);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var author = await _authorService.GetByIdAsync(id.Value);
            if (author == null) return NotFound();

            return View(author);
        }

        public IActionResult Create() => View(new AuthorFormModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthorFormModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _authorService.CreateAsync(model);
            TempData["Success"] = "Author added successfully!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var model = await _authorService.GetFormModelByIdAsync(id.Value);
            if (model == null) return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AuthorFormModel model)
        {
            if (id != model.Id) return NotFound();
            if (!ModelState.IsValid) return View(model);

            var updated = await _authorService.UpdateAsync(model);
            if (!updated) return NotFound();

            TempData["Success"] = "Author updated successfully!";
            return RedirectToAction(nameof(Details), new { id });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var author = await _authorService.GetByIdAsync(id.Value);
            if (author == null) return NotFound();

            return View(author);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _authorService.HasBooksAsync(id))
            {
                TempData["Error"] = "Cannot delete an author who has books in the system.";
                return RedirectToAction(nameof(Details), new { id });
            }

            await _authorService.DeleteAsync(id);
            TempData["Success"] = "Author deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}