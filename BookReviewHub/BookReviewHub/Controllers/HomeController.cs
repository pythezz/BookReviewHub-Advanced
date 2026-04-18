using Microsoft.AspNetCore.Mvc;

namespace BookReviewHub.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

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