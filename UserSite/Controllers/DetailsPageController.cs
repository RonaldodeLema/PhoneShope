using Microsoft.AspNetCore.Mvc;

namespace UserSite.Controllers;

public class DetailsPageController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}