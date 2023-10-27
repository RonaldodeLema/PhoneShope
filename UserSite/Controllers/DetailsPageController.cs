using Internals.Services;
using Internals.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace UserSite.Controllers;

public class DetailsPageController : BaseController
{
    private readonly IPhoneDetailService _phoneDetailService;

    public DetailsPageController(IPhoneDetailService phoneDetailService)
    {
        _phoneDetailService = phoneDetailService;
    }
    // GET
    public async Task<IActionResult> Index(int id)
    {
        var phones = await _phoneDetailService.GetAll();
        var smallestQuantityPhones = phones.OrderBy(phone => phone.Quantity).Take(5).ToList();
        var detailPage = new DetailPage()
        {
            BestSellers = smallestQuantityPhones,
            PhoneDetails = await _phoneDetailService.GetById(id)
        };
        return View(detailPage);
    }
}