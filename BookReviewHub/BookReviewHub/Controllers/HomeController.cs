using BookReviewHub.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookReviewHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookService _bookService;

        public HomeController(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<IActionResult> Index()
        {
            var featured = await _bookService.GetPagedAsync(null, null, "rating", 1, 6);
            return View(featured);
        }

        [Route("not-found")]
        public IActionResult NotFound404()
        {
            Response.StatusCode = 404;
            return View("NotFound");
        }

        [Route("server-error")]
        public IActionResult ServerError()
        {
            Response.StatusCode = 500;
            return View("ServerError");
        }
    }
}