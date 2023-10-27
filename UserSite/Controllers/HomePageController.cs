using System.Diagnostics;
using Internals.Models;
using Internals.Services;
using Internals.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace UserSite.Controllers;

public class HomePageController : BaseController
{
    private readonly ILogger<HomePageController> _logger;
    private readonly IPhoneDetailService _phoneDetailService;
    
    public HomePageController(ILogger<HomePageController> logger, IPhoneDetailService phoneDetailService)
    {
        _logger = logger;
        _phoneDetailService = phoneDetailService;

    }

    public async Task<IActionResult> Index()
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var code = new string(Enumerable.Repeat(chars, 8)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        var phones = await _phoneDetailService.GetAll();
        var smallestQuantityPhones = phones.OrderBy(phone => phone.Quantity).Take(5).ToList();
        var newProducts = phones.OrderBy(p => p.CreatedAt).Take(16).ToList();
        var homePage = new HomePage()
        {
            BestSellers = smallestQuantityPhones,
            NewPhones = newProducts,
            Code = code
        };
        return View(homePage);
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