using BookReviewHub.Services.Interfaces;
using BookReviewHub.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookReviewHub.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IGenreService _genreService;

        public BooksController(IBookService bookService, IGenreService genreService)
        {
            _bookService = bookService;
            _genreService = genreService;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _bookService.GetAllAsync();
            return View(books);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var book = await _bookService.GetByIdAsync(id.Value);
            if (book == null) return NotFound();

            return View(book);
        }

        public async Task<IActionResult> Create()
        {
            var model = new BookFormModel
            {
                Genres = await _genreService.GetSelectListAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Genres = await _genreService.GetSelectListAsync();
                return View(model);
            }

            await _bookService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var model = await _bookService.GetFormModelByIdAsync(id.Value);
            if (model == null) return NotFound();

            model.Genres = await _genreService.GetSelectListAsync();
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
                return View(model);
            }

            var updated = await _bookService.UpdateAsync(model);
            if (!updated) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var book = await _bookService.GetByIdAsync(id.Value);
            if (book == null) return NotFound();

            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _bookService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
