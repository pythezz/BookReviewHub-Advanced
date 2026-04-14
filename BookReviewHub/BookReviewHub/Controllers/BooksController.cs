using BookReviewHub.Services;
using BookReviewHub.Services.Interfaces;
using BookReviewHub.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookReviewHub.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IGenreService _genreService;
        private readonly IAuthorService _authorService; 
        private readonly IReviewService _reviewService;

        public BooksController(
            IBookService bookService,
            IGenreService genreService,
            IAuthorService authorService,
            IReviewService reviewService)
        {
            _bookService = bookService;
            _genreService = genreService;
            _authorService = authorService;
            _reviewService = reviewService;
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

            var reviews = await _reviewService.GetByBookAsync(id.Value);
            ViewBag.Reviews = reviews;

            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                ViewBag.HasReviewed = await _reviewService.UserHasReviewedBookAsync(id.Value, userId);
            }

            return View(book);
        }
        public async Task<IActionResult> Create()
        {
            var model = new BookFormModel
            {
                Genres = await _genreService.GetSelectListAsync(),
                Authors = await _authorService.GetSelectListAsync()
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
                model.Authors = await _authorService.GetSelectListAsync();
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