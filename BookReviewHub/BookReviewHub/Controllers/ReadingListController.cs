using BookReviewHub.Models;
using BookReviewHub.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookReviewHub.Controllers
{
    [Authorize]
    public class ReadingListController : Controller
    {
        private readonly IReadingListService _readingListService;

        public ReadingListController(IReadingListService readingListService)
        {
            _readingListService = readingListService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var list = await _readingListService.GetByUserAsync(userId);
            return View(list);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int bookId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var added = await _readingListService.AddAsync(bookId, userId);

            TempData[added ? "Success" : "Error"] = added
                ? "Book added to your reading list!"
                : "This book is already in your reading list.";

            return RedirectToAction("Details", "Books", new { id = bookId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int bookId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _readingListService.RemoveAsync(bookId, userId);

            TempData["Success"] = "Book removed from your reading list.";
            return RedirectToAction("Details", "Books", new { id = bookId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int bookId, ReadingStatus status)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _readingListService.UpdateStatusAsync(bookId, userId, status);

            TempData["Success"] = "Reading status updated.";
            return RedirectToAction(nameof(Index));
        }
    }
}