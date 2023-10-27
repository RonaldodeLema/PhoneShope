using Internals.Models;
using Internals.Services;
using Internals.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UserSite.Services;

namespace UserSite.Controllers;

public class CartPageController : BaseController
{

    private readonly RedisService _redisService;
    private string? _key;
    private readonly IPhoneDetailService _phoneDetailService;
    public CartPageController(RedisService redisService, IPhoneDetailService phoneDetailService)
    {
        _redisService = redisService;
        _phoneDetailService = phoneDetailService;
    }

    // GET
    public  IActionResult Index()
    {
        return View();
    }
    public IActionResult AddToCart()
    {
        return RedirectToAction("Index");

    }
    [HttpPost]
    public async Task<IActionResult> AddToCart(CartModel cartModel)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Anonymous")))
        {
            HttpContext.Session.SetString("Anonymous", Guid.NewGuid().ToString());
        }
        _key = HttpContext.Session.GetString("Anonymous");
        var listItemsString = await _redisService.GetValue(_key!);
        if(string.IsNullOrEmpty(listItemsString))
        {
            var listModelCart = new List<CartModel> { cartModel };
            var listJson = JsonConvert.SerializeObject(listModelCart);
            await _redisService.SetValue(_key!, listJson, 30);
            return RedirectToAction("Index");
        }

        var flag = true;
        var currentModelCarts = JsonConvert.DeserializeObject<List<CartModel>>(listItemsString);
        foreach (var models in currentModelCarts!.Where(models => models.PhoneDetailId == cartModel.PhoneDetailId))
        {
            models.Quantity += cartModel.Quantity;
            flag = false;
            break;
        }

        if (flag)
        {
            currentModelCarts?.Add(cartModel);
        }
        var currentListJson = JsonConvert.SerializeObject(currentModelCarts);
        await _redisService.SetValue(_key!, currentListJson, 30);
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> GetCarts()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Anonymous")))
        {
            HttpContext.Session.SetString("Anonymous", Guid.NewGuid().ToString());
        }
        _key = HttpContext.Session.GetString("Anonymous");
        
        var listPhoneDetails = new List<PhoneDetails>();
        var listItemsString = await _redisService.GetValue(_key!);
        if(string.IsNullOrEmpty(listItemsString))
        {
            return PartialView(new List<PhoneDetails>());
        }
        var currentModelCarts = JsonConvert.DeserializeObject<List<CartModel>>(listItemsString);
        foreach (var cartModels in currentModelCarts!)
        {
            var phoneDetails = await _phoneDetailService.GetById(cartModels.PhoneDetailId);
            phoneDetails.Quantity = cartModels.Quantity;
            listPhoneDetails.Add(phoneDetails);
        }
        return PartialView(listPhoneDetails);
    }

    public async Task<IActionResult> DeleteById(int id)
    {
        _key = HttpContext.Session.GetString("Anonymous");
        var listItemsString = await _redisService.GetValue(_key!);
        if(string.IsNullOrEmpty(listItemsString))
        {
            return await (Task<JsonResult>)Task.FromException(new Exception("Error"));
        }
        var currentModelCarts = JsonConvert.DeserializeObject<List<CartModel>>(listItemsString);
        CartModel deleteCart = null;
        var flag = false;
        foreach (var cartModels in currentModelCarts!.Where(cartModels => cartModels.PhoneDetailId == id))
        {
            deleteCart = cartModels;
            flag = true;
            break;
        }

        if (!flag) return await (Task<JsonResult>)Task.FromException(new Exception("Error"));
        if (deleteCart != null) currentModelCarts?.Remove(deleteCart);
        var currentListJson = JsonConvert.SerializeObject(currentModelCarts);
        await _redisService.SetValue(_key!, currentListJson, 30);
        return await Task.FromResult(new JsonResult(currentListJson));
    }

    public async Task<IActionResult> UpdateById(int id, int quantity)
    {
        _key = HttpContext.Session.GetString("Anonymous");
        
        var listItemsString = await _redisService.GetValue(_key!);
        if(string.IsNullOrEmpty(listItemsString))
        {
            return await (Task<JsonResult>)Task.FromException(new Exception("Error"));
        }
        var currentModelCarts = JsonConvert.DeserializeObject<List<CartModel>>(listItemsString);
        foreach (var cartModels in currentModelCarts!.Where(cartModels => cartModels.PhoneDetailId == id))
        {
            cartModels.Quantity = quantity;
            break;
        }
        var currentListJson = JsonConvert.SerializeObject(currentModelCarts);
        await _redisService.SetValue(_key!, currentListJson, 30);
        return await Task.FromResult(new JsonResult(currentListJson));
    }
}