using System;
using System.Linq;
using System.Threading.Tasks;
using Internals.Services;
using Internals.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UserSite.Security;
using UserSite.Services;

namespace UserSite.Controllers;

public class AuthPageController : BaseController
{
    private readonly IUserService _userService;
    private readonly IOrderService _orderService;
    private readonly IEmailService _emailService;

    public AuthPageController(IUserService userService, IOrderService orderService,IEmailService emailService)
    {
        _userService = userService;
        _orderService = orderService;
        _emailService = emailService;
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

    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index","HomePage");
        }
        return View("Index");
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
        if (userRegister.Email != null && await _userService.FindByEmail(userRegister.Email)!=null)
        {
            TempData["error"] = "Your email have been registered";
            return View(userRegister);
        }

        var checkUsername = await _userService.FindByUsername(userRegister.Username!);
        if (checkUsername != null)
        {
            TempData["error"] = "Username was exist";
            return View(userRegister);
        }
        var user = await _userService.Register(userRegister);
        TempData["success"] = "Register account success, please check your email to active account";
        var resetToken = Guid.NewGuid().ToString();
        user.ResetToken = resetToken;
        await _userService.Update(user);
        var callbackUrl = $"/AuthPage/ActiveAccount?email={userRegister.Email}&token={resetToken}";
        await _emailService.SendActiveEmailAsync(userRegister.Email, resetToken, callbackUrl);
        return View();
    }

    public async Task<IActionResult> ActiveAccount(string email, string token)
    {
        var user = await _userService.FindByEmail(email);
        if (user==null||user.ResetToken!=token)
        {
            return RedirectToAction("Index");
        }
        user.IsBlocked = false;
        await _userService.Update(user);
        return View();
    }
    public  IActionResult ResetPassword(string? email, string? token)
    {
        if (email == null || token == null)
        {
            return RedirectToAction("SendResetEmail");
        }
        return View(new ResetPasswordModel
        {
            NewPassword = "",
            ReNewPassword = "",
            ResetToken = token,
            Email = email
        });
    }
    [HttpPost]
    [Obsolete("Obsolete")]
    public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
    {
        var user = await _userService.FindByEmail(resetPasswordModel.Email);
        if (resetPasswordModel.NewPassword == resetPasswordModel.ReNewPassword)
        {
            resetPasswordModel.HashPassword();
        }
        else
        {
            TempData["info"] = "Enter renew password not equal new password";
            return View("ResetPassword", resetPasswordModel);
        }
        if (user.ResetToken == resetPasswordModel.ResetToken)
        {
            user.Password = resetPasswordModel.NewPassword;
            await _userService.Update(user);
            TempData["info"] = "Reset password success";
            resetPasswordModel.ResetToken = "";
            return RedirectToAction("Index");
        }
        TempData["info"] = "You don't verify email";
        return View("ResetPassword",resetPasswordModel);
    }
    public  IActionResult SendResetEmail()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> SendResetEmail(string email)
    {
        var user = await _userService.FindByEmail(email);
        if (user != null)
        {
            var resetToken = Guid.NewGuid().ToString();
            user.ResetToken = resetToken;
            await _userService.Update(user);
            var callbackUrl = $"/AuthPage/ResetPassword?email={email}&token={resetToken}";
            await _emailService.SendPasswordResetEmailAsync(email, resetToken, callbackUrl);
            TempData["info"] = "Please check mail for reset password";
            return View("SendResetEmail");
        }
        TempData["info"] = "Email not valid";
        return View("SendResetEmail");
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