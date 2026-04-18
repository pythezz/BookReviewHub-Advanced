using BookReviewHub.Services.Interfaces;
using BookReviewHub.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookReviewHub.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IGenreService _genreService;
        private readonly IAuthorService _authorService;

        public BooksController(
            IBookService bookService,
            IGenreService genreService,
            IAuthorService authorService)
        {
            _bookService = bookService;
            _genreService = genreService;
            _authorService = authorService;
        }

        public async Task<IActionResult> Index(string? search, int page = 1)
        {
            var books = await _bookService.GetPagedAsync(search, null, null, page, 10);
            ViewBag.Search = search;
            return View(books);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var model = await _bookService.GetFormModelByIdAsync(id.Value);
            if (model == null) return NotFound();

            model.Genres = await _genreService.GetSelectListAsync();
            model.Authors = await _authorService.GetSelectListAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BookFormModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                model.Genres = await _genreService.GetSelectListAsync();
                model.Authors = await _authorService.GetSelectListAsync();
                return View(model);
            }

            await _bookService.UpdateAsync(model);
            TempData["Success"] = "Book updated.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _bookService.DeleteAsync(id);
            TempData["Success"] = "Book deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}