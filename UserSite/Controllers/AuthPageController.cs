using System.Security.Cryptography;
using Internals.Models;
using Internals.Services;
using Internals.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UserSite.Security;

namespace UserSite.Controllers;

public class AuthPageController : BaseController
{
    private readonly IUserService _userService;
    private readonly IOrderService _orderService;

    public AuthPageController(IUserService userService, IOrderService orderService)
    {
        _userService = userService;
        _orderService = orderService;
    }
    // GET
    public IActionResult Index()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index","HomePage");
        }
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(UserLogin userLogin)
    {
        if (!ModelState.IsValid)
        {
            return View("Index");
        }
        var pwd = userLogin.Password;
        var user = await _userService.Login(userLogin);
        if (user == null)
        {
            TempData["error"] = "Username or password is invalid";
            return View("Index", userLogin);
        }
        if (userLogin.RememberMe)
        {
            userLogin.Password = pwd;
            userLogin.Password = userLogin.EncryptPassword();
            HttpContext.Session.SetString("RememberMe",JsonConvert.SerializeObject(userLogin));
        }
        CreateAuthenticationTicket(user);
        user.SanitizePassword();
        return RedirectToAction("Index","HomePage");
    }

    public Task<IActionResult> RememberMe()
    {
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("RememberMe")))
        {
            return Task.FromResult<IActionResult>(new JsonResult(JsonConvert.SerializeObject(new UserLogin())));
        }
        var userJson = HttpContext.Session.GetString("RememberMe");
        var userLogin = JsonConvert.DeserializeObject<UserLogin>(userJson!);
        return Task.FromResult<IActionResult>(new JsonResult(JsonConvert.SerializeObject(userLogin)));
    }
    // GET
    public IActionResult SignUp()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> SignUp(UserRegister userRegister)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        if (!userRegister.ComparePassword())
        {
            TempData["error"] = "Password again not equal password";
            return View(userRegister);
        }

        var checkUsername = await _userService.FindByUsername(userRegister.Username!);
        if (checkUsername != null)
        {
            TempData["error"] = "Username was exist";
            return View(userRegister);
        }
        await _userService.Register(userRegister);
        TempData["success"] = "Register account success";
        return View();
    }

    public async Task<IActionResult> CollectDeviceToken(string deviceToken)
    {
        if (!HttpContext.User.Identity!.IsAuthenticated)
            return new JsonResult("add DeviceToken failed: " + deviceToken);
        var user = await _userService.FindByUsername(HttpContext.User.Identity.Name!);
        if (user!.DeviceToken == deviceToken) return new JsonResult("add DeviceToken failed: " + deviceToken);
        user.DeviceToken = deviceToken;
        await _userService.Update(user);
        return new JsonResult("add DeviceToken success:"+deviceToken);
    }

    [NonAction]
    [Authorize("user")]
    private async Task<ProfilePage> GetProfile()
    {
        var username = HttpContext.User.Identity.Name;
        var user = await _userService.FindByUsername(username);
        var orders = await _orderService.GetOrderByUserId(user.Id);
        var profilePage = new ProfilePage
        {
            UserUpdate = new UserUpdate
            {
                Name = user.Name,
                Email = user.Email,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber
            },
            Orders = orders.OrderByDescending(p=>p.CreatedAt).ToList(),
            ChangePassword = new ChangePassword()
        };
        return profilePage;
    }
    [Authorize("user")]
    public async Task<IActionResult> Profile()
    {
        return View( await GetProfile());
    }
    
    [Authorize("user")]
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("JWToken");
        return RedirectToAction("Index", "HomePage");
    }
    [Authorize("user")]
    public async Task<IActionResult> DetailOrders(int id)
    {
        var phone = await _orderService.GetById(id);

        return View(phone);
    }

    [Authorize("user")]
    public async Task<IActionResult> ChangePassword()
    {
        return View("Profile",await GetProfile());
    }

    [Authorize("user")]
    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePassword changePassword)
    {
        if (changePassword.NewPassword.Equals(changePassword.OldPassword))
        {
            TempData["error"] = "Old password equal new password";
            return View("Profile",await GetProfile());
        }
        
        if (!changePassword.NewPassword.Equals(changePassword.ReNewPassword))
        {
            TempData["error"] = "New password and Renew password not valid";
            return View("Profile",await GetProfile());
        }
        var username = HttpContext.User.Identity.Name;
        var user = await _userService.FindByUsername(username);
        changePassword.HashPassword();
        if (!user.ComparePasswords(changePassword.OldPassword))
        {
            TempData["error"] = "Old password not valid";
            return View("Profile",await GetProfile());
        }

        user.Password = changePassword.NewPassword;
        await _userService.Update(user);
        TempData["success"] = "Change password success";
        return View("Profile",await GetProfile());
    }
    
    [Authorize("user")]
    public async Task<IActionResult> UpdateProfile()
    {
        return View("Profile",await GetProfile());
    }
    [Authorize("user")]
    [HttpPost]
    public async Task<IActionResult> UpdateProfile(UserUpdate userUpdate)
    {
        if (!ModelState.IsValid)
        {
            TempData["error-profile"] = "Form have field empty";
            return View("Profile",await GetProfile());
        }
        var username = HttpContext.User.Identity.Name;
        var user = await _userService.FindByUsername(username);
        user.Name = userUpdate.Name;
        user.PhoneNumber = userUpdate.PhoneNumber;
        user.Email = userUpdate.Email;
        user.Address = userUpdate.Address;
        await _userService.Update(user);
        TempData["success-profile"] = "Change profile successfully";
        return View("Profile",await GetProfile());
    }
}