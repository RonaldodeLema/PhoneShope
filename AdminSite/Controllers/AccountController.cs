using System.Security.Claims;
using Internals.Repository;
using Internals.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace AdminSite.Controllers;

public class AccountController : Controller
{
    private readonly IAdminRepository _repository;
    public AccountController(IAdminRepository repository)
    {
        _repository = repository;
    }
    public IActionResult Index()
    {
        if (User.Identity?.Name != null)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }
    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity?.Name != null)
        {
            RedirectToAction("Index", "Home");
        }
        return View("Index");
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult Login([Bind("Username,Password,RememberMe")] UserLogin model)
    {
        var taskUser = _repository.Login(model);
        if (taskUser?.Result == null)
        {
            ModelState.AddModelError("ErrorMes", "Username or password is wrong");
            return View("Index", model);
        }

        if (model.RememberMe)
        {
           
        }
        var identity = new ClaimsIdentity(new[] {  
            new Claim(ClaimTypes.Name, taskUser.Result.Username),  
            new Claim(ClaimTypes.Role, taskUser.Result.Role.Name)  
        }, CookieAuthenticationDefaults.AuthenticationScheme);  
  
        var isAuthenticated = true;

        if (!isAuthenticated) return View("Index", model);
        var principal = new ClaimsPrincipal(identity);  
  
        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);  
  
        return RedirectToAction("Index", "Home");
    }
    public IActionResult AccessDenied()
    {
        return View();
    }
    public IActionResult Logout()
    {
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }  
}