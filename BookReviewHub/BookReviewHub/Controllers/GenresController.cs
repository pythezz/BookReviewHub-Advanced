using BookReviewHub.Services.Interfaces;
using BookReviewHub.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookReviewHub.Controllers
{
    public class GenresController : Controller
    {
        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        public async Task<IActionResult> Index()
        {
            var genres = await _genreService.GetAllAsync();
            return View(genres);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var genre = await _genreService.GetByIdAsync(id.Value);
            if (genre == null) return NotFound();

            return View(genre);
        }

        public IActionResult Create()
        {
            return View(new GenreFormModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GenreFormModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _genreService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var model = await _genreService.GetFormModelByIdAsync(id.Value);
            if (model == null) return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GenreFormModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            var updated = await _genreService.UpdateAsync(model);
            if (!updated) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var genre = await _genreService.GetByIdAsync(id.Value);
            if (genre == null) return NotFound();

            return View(genre);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _genreService.HasBooksAsync(id))
            {
                var genre = await _genreService.GetByIdAsync(id);
                ModelState.AddModelError("", "Cannot delete this genre because it has books assigned to it.");
                return View(genre);
            }

            await _genreService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
