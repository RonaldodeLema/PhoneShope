using Internals.Models;
using Internals.Models.Enum;
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
    private readonly IOrderService _orderService;

    public OrderPageController(RedisService redisService, IUserService userService,
        IPhoneDetailService phoneDetailService, IPaymentService paymentService, IPromotionService promotionService,
        IOrderService orderService)
    {
        _redisService = redisService;
        _userService = userService;
        _phoneDetailService = phoneDetailService;
        _paymentService = paymentService;
        _promotionService = promotionService;
        _orderService = orderService;
    }

    [NonAction]
    private async Task<OrderPageModel> GetOrderPage()
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var code = new string(Enumerable.Repeat(chars, 10)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        if (HttpContext.User.Identity is { IsAuthenticated: true, Name: not null })
        {
            var username = HttpContext.User.Identity.Name;
            var user = await _userService.FindByUsername(username);
            var listItemsString = await _redisService.GetValue(CartUser + username);
            var listPhoneDetails = new List<PhoneDetails>();
            if (string.IsNullOrEmpty(listItemsString))
            {
                return new OrderPageModel()
                {
                    User = user,
                    PhoneDetailsList = new List<PhoneDetails>(),
                    Payments = await _paymentService.GetAll(),
                    OrderCode = code,
                    ReqOrderModel = new ReqOrderModel(),
                    Promotions = await _promotionService.GetsByUserId(user.Id)
                };
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
            return orderPageModel;
        }

        return new OrderPageModel();
    }

    public async Task<IActionResult> Index()
    {
        var orderPageModel = await GetOrderPage();
        return View(orderPageModel);
    }

    public IActionResult Checkout()
    {
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Checkout(ReqOrderModel reqOrderModel)
    {
        if (HttpContext.User.Identity is { IsAuthenticated: true, Name: not null })
        {
            var username = HttpContext.User.Identity.Name;
            var user = await _userService.FindByUsername(username);
            var listItemsString = await _redisService.GetValue(CartUser + username);
            if (string.IsNullOrEmpty(listItemsString))
            {
                TempData["error"] = "Your Cart Is Empty";
                return View("Index", await GetOrderPage());
            }


            var order = await _orderService.AddAsync(new Order()
            {
                UserId = user!.Id,
                Status = Status.Depending,
                Method = reqOrderModel.Method,
                PromotionId = reqOrderModel.Promotion.Id,
                Note = reqOrderModel.Note,
                PayCode = reqOrderModel.PayCode,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });
            var promotion = await _promotionService.FindById(reqOrderModel.Promotion.Id);
            promotion.IsUsed = true;
            await _promotionService.UpdatePromotion(promotion);
            var currentModelCarts = JsonConvert.DeserializeObject<List<CartModel>>(listItemsString);
            foreach (var cartModels in currentModelCarts!)
            {
                var phoneDetails = await _phoneDetailService.GetById(cartModels.PhoneDetailId);
                var orderItem = new OrderItem()
                {
                    OrderId = order.Id,
                    PhoneDetailsId = phoneDetails.Id,
                    Quantity = cartModels.Quantity,
                    Price = phoneDetails.Price
                };
                await _orderService.AddOrderItemsAsync(orderItem);
                phoneDetails.Quantity -= cartModels.Quantity;
                await _phoneDetailService.UpdateAsync(phoneDetails);
            }

            await _redisService.Remove(CartUser + username);
        }

        TempData["success"] = "Your Order Successfully";
        return View("Index", await GetOrderPage());
    }
}