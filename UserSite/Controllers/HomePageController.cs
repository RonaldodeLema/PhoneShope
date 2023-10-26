using System.Diagnostics;
using Internals.Models;
using Microsoft.AspNetCore.Mvc;

namespace UserSite.Controllers;

public class HomePageController : Controller
{
    private readonly ILogger<HomePageController> _logger;

    public HomePageController(ILogger<HomePageController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Anonymous")))
        {
            HttpContext.Session.SetString("Anonymous", Guid.NewGuid().ToString());
        }
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}