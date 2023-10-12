using Internals.Models;
using Internals.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminSite.Controllers;

[Authorize("Manage_User")]
public class UserController : Controller
{
    private readonly IRepository<User,int> _repository;

    public UserController(IRepository<User,int> repository)
    {
       
        _repository = repository;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _repository.GetAllAsync();
        return View(users);
    }
    public async Task<IActionResult> Details(int id)
    {
        var user = await _repository.GetByIdAsync(id);

        return View(user);
    }
}