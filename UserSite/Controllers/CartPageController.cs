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
        var flag = true;
        List<CartModel>? currentModelCarts;
        string currentListJson;
        // Authenticated
        if (HttpContext.User.Identity is { IsAuthenticated: true, Name: not null })
        {
            var username = HttpContext.User.Identity.Name;
            var listItemOfUser = await _redisService.GetValue(CartUser+username);
            if(string.IsNullOrEmpty(listItemOfUser))
            {
                var listModelCart = new List<CartModel> { cartModel };
                var listJson = JsonConvert.SerializeObject(listModelCart);
                await _redisService.SetValue(CartUser+username, listJson, 14);
                return RedirectToAction("Index");
            }
            currentModelCarts = JsonConvert.DeserializeObject<List<CartModel>>(listItemOfUser);
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
            currentListJson = JsonConvert.SerializeObject(currentModelCarts);
            await _redisService.SetValue(CartUser+username, currentListJson, 14);
            return RedirectToAction("Index");
        }
        
        // Anonymous
        if (string.IsNullOrEmpty(HttpContext.Session.GetString(CartAnonymous)))
        {
            HttpContext.Session.SetString(CartAnonymous, CartAnonymous+Guid.NewGuid());
        }
        _key = HttpContext.Session.GetString(CartAnonymous);
        var listItemsString = await _redisService.GetValue(_key!);
        if(string.IsNullOrEmpty(listItemsString))
        {
            var listModelCart = new List<CartModel> { cartModel };
            var listJson = JsonConvert.SerializeObject(listModelCart);
            await _redisService.SetValue(_key!, listJson, 1);
            return RedirectToAction("Index");
        }

        flag = true;
        currentModelCarts = JsonConvert.DeserializeObject<List<CartModel>>(listItemsString);
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
        currentListJson = JsonConvert.SerializeObject(currentModelCarts);
        await _redisService.SetValue(_key!, currentListJson, 1);
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> GetCarts()
    {
        var listPhoneDetails = new List<PhoneDetails>();
        string? listItemsString;
        List<CartModel>? currentModelCarts;
        //Authenticated
        if (HttpContext.User.Identity is { IsAuthenticated: true, Name: not null })
        {
            listItemsString = await _redisService.GetValue(CartUser+HttpContext.User.Identity.Name);
            if(string.IsNullOrEmpty(listItemsString))
            {
                return PartialView(new List<PhoneDetails>());
            }
            currentModelCarts = JsonConvert.DeserializeObject<List<CartModel>>(listItemsString);
            foreach (var cartModels in currentModelCarts!)
            {
                var phoneDetails = await _phoneDetailService.GetById(cartModels.PhoneDetailId);
                phoneDetails.Quantity = cartModels.Quantity;
                listPhoneDetails.Add(phoneDetails);
            }
            return PartialView(listPhoneDetails);
        }
        
        //Anonymous
        if (string.IsNullOrEmpty(HttpContext.Session.GetString(CartAnonymous)))
        {
            HttpContext.Session.SetString(CartAnonymous, CartAnonymous+Guid.NewGuid());
        }
        _key = HttpContext.Session.GetString(CartAnonymous);
        
        listPhoneDetails = new List<PhoneDetails>();
        listItemsString = await _redisService.GetValue(_key!);
        if(string.IsNullOrEmpty(listItemsString))
        {
            return PartialView(new List<PhoneDetails>());
        }
        currentModelCarts = JsonConvert.DeserializeObject<List<CartModel>>(listItemsString);
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
        string currentListJson;
        List<CartModel>? currentModelCarts;
        string? listItemsString;
        CartModel deleteCart;
        bool flag;
        //Authenticated
        if (HttpContext.User.Identity is { IsAuthenticated: true, Name: not null })
        {
            listItemsString = await _redisService.GetValue(CartUser+HttpContext.User.Identity.Name);
            if(string.IsNullOrEmpty(listItemsString))
            {
                return await (Task<JsonResult>)Task.FromException(new Exception("Error"));
            }
            currentModelCarts = JsonConvert.DeserializeObject<List<CartModel>>(listItemsString);
            deleteCart = null;
            flag = false;
            foreach (var cartModels in currentModelCarts!.Where(cartModels => cartModels.PhoneDetailId == id))
            {
                deleteCart = cartModels;
                flag = true;
                break;
            }

            if (!flag) return await (Task<JsonResult>)Task.FromException(new Exception("Error"));
            if (deleteCart != null) currentModelCarts?.Remove(deleteCart);
            currentListJson = JsonConvert.SerializeObject(currentModelCarts);
            await _redisService.SetValue(CartUser+HttpContext.User.Identity.Name, currentListJson, 14);
            return await Task.FromResult(new JsonResult(currentListJson));
        }
        //Anonymous
        _key = HttpContext.Session.GetString(CartAnonymous);
         listItemsString = await _redisService.GetValue(_key!);
        if(string.IsNullOrEmpty(listItemsString))
        {
            return await (Task<JsonResult>)Task.FromException(new Exception("Error"));
        }
        currentModelCarts = JsonConvert.DeserializeObject<List<CartModel>>(listItemsString);
        deleteCart = null;
        flag = false;
        foreach (var cartModels in currentModelCarts!.Where(cartModels => cartModels.PhoneDetailId == id))
        {
            deleteCart = cartModels;
            flag = true;
            break;
        }

        if (!flag) return await (Task<JsonResult>)Task.FromException(new Exception("Error"));
        if (deleteCart != null) currentModelCarts?.Remove(deleteCart);
        currentListJson = JsonConvert.SerializeObject(currentModelCarts);
        await _redisService.SetValue(_key!, currentListJson, 1);
        return await Task.FromResult(new JsonResult(currentListJson));
    }

    public async Task<IActionResult> UpdateById(int id, int quantity)
    {
        string? listItemsString;
        string currentListJson;
        List<CartModel>? currentModelCarts;
        
        //Authenticated
        if (HttpContext.User.Identity is { IsAuthenticated: true, Name: not null })
        {
            listItemsString = await _redisService.GetValue(CartUser+HttpContext.User.Identity.Name);

            if(string.IsNullOrEmpty(listItemsString))
            {
                return await (Task<JsonResult>)Task.FromException(new Exception("Error"));
            }
            currentModelCarts = JsonConvert.DeserializeObject<List<CartModel>>(listItemsString);
            foreach (var cartModels in currentModelCarts!.Where(cartModels => cartModels.PhoneDetailId == id))
            {
                cartModels.Quantity = quantity;
                break;
            }
            currentListJson = JsonConvert.SerializeObject(currentModelCarts);
            await _redisService.SetValue(CartUser+HttpContext.User.Identity.Name, currentListJson, 14);
            return await Task.FromResult(new JsonResult(currentListJson));
        }

        //Anonymous
        _key = HttpContext.Session.GetString(CartAnonymous);
        listItemsString = await _redisService.GetValue(_key!);
        if(string.IsNullOrEmpty(listItemsString))
        {
            return await (Task<JsonResult>)Task.FromException(new Exception("Error"));
        }
        currentModelCarts = JsonConvert.DeserializeObject<List<CartModel>>(listItemsString);
        foreach (var cartModels in currentModelCarts!.Where(cartModels => cartModels.PhoneDetailId == id))
        {
            cartModels.Quantity = quantity;
            break;
        }
        currentListJson = JsonConvert.SerializeObject(currentModelCarts);
        await _redisService.SetValue(_key!, currentListJson, 1);
        return await Task.FromResult(new JsonResult(currentListJson));
    }
}