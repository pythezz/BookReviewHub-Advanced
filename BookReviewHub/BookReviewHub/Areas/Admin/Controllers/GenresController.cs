using BookReviewHub.Services.Interfaces;
using BookReviewHub.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookReviewHub.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
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

        [HttpGet]
        public IActionResult Create() => View(new GenreFormModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GenreFormModel model)
        {
            if (!ModelState.IsValid) return View(model);
            await _genreService.CreateAsync(model);
            TempData["Success"] = "Genre created.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
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
            if (!ModelState.IsValid) return View(model);
            await _genreService.UpdateAsync(model);
            TempData["Success"] = "Genre updated.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _genreService.HasBooksAsync(id))
            {
                TempData["Error"] = "Cannot delete a genre that has books assigned to it.";
                return RedirectToAction(nameof(Index));
            }
            await _genreService.DeleteAsync(id);
            TempData["Success"] = "Genre deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}