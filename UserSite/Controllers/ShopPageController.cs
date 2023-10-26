using Internals.Models;
using Internals.Models.Enum;
using Internals.Repository;
using Internals.Services;
using Internals.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace UserSite.Controllers;

public class ShopPageController : Controller
{
    private readonly IPhoneDetailService _phoneDetailService;

    public ShopPageController(IPhoneDetailService phoneDetailService)
    {
        _phoneDetailService = phoneDetailService;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var shopPage = new ShopPage()
        {
            TotalPhone = _phoneDetailService.GetAll().Result.Count,
            Categories = await _phoneDetailService.GetAllCategory(),
            QuantityDisplay = 9
        };
        return View(shopPage);
    }


    [HttpPost]
    public async Task<IActionResult> BreakPage([FromBody] FilterModel filterModel)
    {
        var allPhones = await _phoneDetailService.GetAll();
        if (filterModel.IsEmpty())
        {
            return PartialView(allPhones.GetRange((filterModel.PageNum - 1) * filterModel.Quantity, filterModel.Quantity));
        }

        var filterPhones = await _phoneDetailService.GetByFilter(filterModel);
        
        var quantity = filterModel.Quantity;
        if (filterModel.Quantity > filterPhones.Count-(filterModel.PageNum - 1) * quantity)
        {
            quantity = filterPhones.Count-(filterModel.PageNum - 1) * quantity;
        }
        Console.WriteLine(filterPhones.Count);
        return PartialView(filterPhones.GetRange((filterModel.PageNum - 1) * quantity, quantity));
    }
}