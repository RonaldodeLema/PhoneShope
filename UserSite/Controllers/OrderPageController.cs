using Internals.Models;
using Internals.Services;
using Internals.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UserSite.Security;
using UserSite.Services;

namespace UserSite.Controllers;

[Authorize("user")]
public class OrderPageController : BaseController
{
    private readonly RedisService _redisService;
    private readonly IPaymentService _paymentService;
    private readonly IUserService _userService;
    private readonly IPhoneDetailService _phoneDetailService;
    private readonly IPromotionService _promotionService;
    public OrderPageController(RedisService redisService, IUserService userService, IPhoneDetailService phoneDetailService, IPaymentService paymentService, IPromotionService promotionService)
    {
        _redisService = redisService;
        _userService = userService;
        _phoneDetailService = phoneDetailService;
        _paymentService = paymentService;
        _promotionService = promotionService;
    }
    // GET
    public async Task<IActionResult> Index()
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var code = new string(Enumerable.Repeat(chars, 10)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        if (HttpContext.User.Identity is { IsAuthenticated: true, Name: not null })
        {
            var username = HttpContext.User.Identity.Name;
            var user = await _userService.FindByUsername(username);
            var listItemsString = await _redisService.GetValue(CartUser+username);
            var listPhoneDetails = new List<PhoneDetails>();
            if(string.IsNullOrEmpty(listItemsString))
            {
                return View(new OrderPageModel()
                {
                    User = user,
                    PhoneDetailsList = new List<PhoneDetails>(),
                    Payments = await _paymentService.GetAll(),
                    OrderCode = code,
                    ReqOrderModel = new ReqOrderModel(),
                    Promotions = await _promotionService.GetsByUserId(user.Id)
                });
            }
            var currentModelCarts = JsonConvert.DeserializeObject<List<CartModel>>(listItemsString);
            foreach (var cartModels in currentModelCarts!)
            {
                var phoneDetails = await _phoneDetailService.GetById(cartModels.PhoneDetailId);
                phoneDetails.Quantity = cartModels.Quantity;
                listPhoneDetails.Add(phoneDetails);
            }
            var orderPageModel = new OrderPageModel()
            {
                User = user,
                PhoneDetailsList = listPhoneDetails,
                Payments = await _paymentService.GetAll(),
                OrderCode = code,
                Promotions = await _promotionService.GetsByUserId(user.Id),
                ReqOrderModel = new ReqOrderModel()
            };
            return View(orderPageModel);
        }
        return View();
    }
    [HttpPost]
    public IActionResult Checkout(ReqOrderModel reqOrderModel)
    {
        return RedirectToAction("Index");
    }
}