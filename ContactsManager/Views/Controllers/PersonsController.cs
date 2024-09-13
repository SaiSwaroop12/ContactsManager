using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.Views.Controllers
{
    public class PersonsController : Controller
    {
        [Route("/persons/index")]
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
