using Internals.Services;
using Internals.ViewModels;
using Microsoft.AspNetCore.Mvc;
using UserSite.Security;

namespace UserSite.Controllers;

[Authorize("user")]
public class ShopPageController : BaseController
{
    private readonly IPhoneDetailService _phoneDetailService;

    public ShopPageController(IPhoneDetailService phoneDetailService)
    {
        _phoneDetailService = phoneDetailService;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var phones = await _phoneDetailService.GetAll();
        var smallestQuantityPhones = phones.OrderBy(phone => phone.Quantity).Take(5);
        var shopPage = new ShopPage()
        {
            TotalPhone = _phoneDetailService.GetAll().Result.Count,
            Categories = await _phoneDetailService.GetAllCategory(),
            BestSellers = smallestQuantityPhones.ToList(),
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