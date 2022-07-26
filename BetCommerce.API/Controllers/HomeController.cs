using Microsoft.AspNetCore.Mvc;

namespace BetCommerce.API.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Redirect("/index.html");
        }
    }
}
