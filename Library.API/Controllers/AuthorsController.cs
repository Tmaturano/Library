using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    public class AuthorsController : Controller
    {
        public AuthorsController()
        {

        }
        public IActionResult Index()
        {
            return View();
        }
    }
}