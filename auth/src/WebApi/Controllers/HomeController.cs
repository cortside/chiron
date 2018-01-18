using Microsoft.AspNetCore.Mvc;

namespace Chiron.Auth.Controllers
{
    public class HomeController : Controller
    {
	public IActionResult Index()
	{
	    return View();
	}

	public IActionResult Error()
	{
	    return View();
	}
    }
}
