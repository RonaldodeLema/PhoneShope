using System.Diagnostics;
using Internals.Models;
using Internals.Services;
using Internals.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UserSite.Services;

namespace UserSite.Controllers;

public class HomePageController : BaseController
{
    private readonly ILogger<HomePageController> _logger;
    private readonly IPhoneDetailService _phoneDetailService;
    private readonly RedisService _redisService;
    
    public HomePageController(ILogger<HomePageController> logger,RedisService redisService, IPhoneDetailService phoneDetailService)
    {
        _redisService = redisService;
        _logger = logger;
        _phoneDetailService = phoneDetailService;

    }

    public async Task<IActionResult> Index()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString(CartAnonymous)))
        {
            if (HttpContext.User.Identity is { IsAuthenticated: true, Name: not null })
            {
                var checkCartOfUser = await _redisService.GetValue(CartUser+HttpContext.User.Identity.Name);
                if (string.IsNullOrEmpty(checkCartOfUser)|| checkCartOfUser.Equals("[]"))
                {
                    var key = HttpContext.Session.GetString(CartAnonymous);
                    var listItemsString = await _redisService.GetValue(key);
                    if (!string.IsNullOrEmpty(listItemsString))
                    {
                        var currentModelCarts = JsonConvert.DeserializeObject<List<CartModel>>(listItemsString);
                        var currentListJson = JsonConvert.SerializeObject(currentModelCarts);
                        await _redisService.SetValue(CartUser+HttpContext.User.Identity.Name,currentListJson, 30);
                    }
                }
            }
        }
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