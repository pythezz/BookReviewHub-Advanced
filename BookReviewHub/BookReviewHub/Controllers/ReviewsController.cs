using BookReviewHub.Services.Interfaces;
using BookReviewHub.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookReviewHub.Controllers
{
    [Authorize]
    public class ReviewsController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReviewFormModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please correct the errors in your review.";
                return RedirectToAction("Details", "Books", new { id = model.BookId });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            if (await _reviewService.UserHasReviewedBookAsync(model.BookId, userId))
            {
                TempData["Error"] = "You have already reviewed this book.";
                return RedirectToAction("Details", "Books", new { id = model.BookId });
            }

            await _reviewService.CreateAsync(model, userId);
            TempData["Success"] = "Review submitted successfully!";
            return RedirectToAction("Details", "Books", new { id = model.BookId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var model = await _reviewService.GetFormModelByIdAsync(id.Value);
            if (model == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var review = await _reviewService.GetByIdAsync(id.Value);

            if (review!.UserId != userId && !User.IsInRole("Administrator"))
                return Forbid();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReviewFormModel model)
        {
            if (id != model.Id) return NotFound();
            if (!ModelState.IsValid) return View(model);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var updated = await _reviewService.UpdateAsync(model, userId);
            if (!updated) return Forbid();

            TempData["Success"] = "Review updated.";
            return RedirectToAction("Details", "Books", new { id = model.BookId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int bookId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var isAdmin = User.IsInRole("Administrator");

            var deleted = await _reviewService.DeleteAsync(id, userId, isAdmin);
            TempData[deleted ? "Success" : "Error"] = deleted
                ? "Review deleted."
                : "You are not allowed to delete this review.";

            return RedirectToAction("Details", "Books", new { id = bookId });
        }

        [HttpGet]
        public async Task<IActionResult> MyReviews()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var reviews = await _reviewService.GetByUserAsync(userId);
            return View(reviews);
        }
    }
}