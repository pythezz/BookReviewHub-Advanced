using BookReviewHub.Services.Interfaces;
using BookReviewHub.ViewModels;
using Microsoft.AspNetCore.Authorization;
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

        private readonly IReadingListService _readingListService;

        public BooksController(
            IBookService bookService,
            IGenreService genreService,
            IAuthorService authorService,
            IReviewService reviewService,
            IReadingListService readingListService)
        {
            _bookService = bookService;
            _genreService = genreService;
            _authorService = authorService;
            _reviewService = reviewService;
            _readingListService = readingListService;
        }

        private const int PageSize = 6;

        public async Task<IActionResult> Index(
            string? search, int? genreId, string? sortBy, int page = 1)
        {
            var books = await _bookService.GetPagedAsync(search, genreId, sortBy, page, PageSize);
            var genres = await _genreService.GetSelectListAsync();

            var vm = new BookSearchViewModel
            {
                SearchTerm = search,
                GenreId = genreId,
                SortBy = sortBy,
                CurrentPage = page,
                Books = books,
                Genres = genres
            };

            return View(vm);
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
                ViewBag.IsInList = await _readingListService.IsInListAsync(id.Value, userId);
            }

            return View(book);
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            var model = new BookFormModel
            {
                Genres = await _genreService.GetSelectListAsync(),
                Authors = await _authorService.GetSelectListAsync()
            };

            return View(model);
        }

        [Authorize]
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
            TempData["Success"] = "Book added successfully!";
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var model = await _bookService.GetFormModelByIdAsync(id.Value);
            if (model == null) return NotFound();
            model.Genres = await _genreService.GetSelectListAsync();
            model.Authors = await _authorService.GetSelectListAsync();
            return View(model);
        }

        [Authorize]
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
            TempData["Success"] = "Book updated successfully!";

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var book = await _bookService.GetByIdAsync(id.Value);
            if (book == null) return NotFound();
            return View(book);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _bookService.DeleteAsync(id);
            TempData["Success"] = "Book deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}