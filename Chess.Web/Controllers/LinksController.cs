using Microsoft.AspNetCore.Mvc;

namespace Chess.Web.Controllers
{
    [Route("links")]
    public class LinksController : Controller
    {

        [HttpGet]
        public IActionResult Home()
        {
            return View();
        }
    }
}
