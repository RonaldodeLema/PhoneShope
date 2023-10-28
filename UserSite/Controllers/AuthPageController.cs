using System.Security.Cryptography;
using Internals.Services;
using Internals.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UserSite.Security;

namespace UserSite.Controllers;

public class AuthPageController : BaseController
{
    private readonly IUserService _userService;
    public AuthPageController(IUserService userService)
    {
        _userService = userService;
    }
    // GET
    public IActionResult Index()
    {
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
    
    [Authorize("user")]
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("JWToken");
        return RedirectToAction("Index", "HomePage");
    }
}