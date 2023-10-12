using Internals.Models;
using Internals.Repository;
using Internals.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminSite.Controllers;
[Authorize("Manage_Admin")]
public class AdminController : Controller
{
    private readonly IAdminRepository _adminRepository;
    private readonly IRepository<Admin,int> _repository;

    public AdminController(IAdminRepository adminRepository,IRepository<Admin,int> repository)
    {
        _adminRepository = adminRepository;
        _repository = repository;
    }
    // GET
    public async Task<IActionResult> Index()
    {
        var admins = await _repository.GetAllAsync();
        return View(admins);
    }
    
    public async Task<IActionResult> Create()
    {
        var admins = await _repository.GetAllAsync();
        return View("Index",admins);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Username,Password,RePassword,Role")] AdminRegister adminRegister)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("Error","Field empty");
            return View("Index", await _repository.GetAllAsync());
        }
        if (!adminRegister.ComparePassword())
        {
            ModelState.AddModelError("Error","Enter again password is wrong");
            return View("Index", await _repository.GetAllAsync());
        }
        
        var checkUsernameExist = _adminRepository.FindByUsername(adminRegister.Username).Result;
        if (checkUsernameExist!=null)
        {
            ModelState.AddModelError("Error","Username is valid");
            return View("Index", await _repository.GetAllAsync());

        }
        var admin = adminRegister.ConvertToAdmin();
        
        await _repository.AddAsync(admin);
        ModelState.AddModelError("Error","Create new admin success");
        return View("Index", await _repository.GetAllAsync());

    }
    public async Task<IActionResult> Delete()
    {
        var admins = await _repository.GetAllAsync();
        return View("Index",admins);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete([Bind("id")]int id)
    {
        var admin = await _repository.GetByIdAsync(id);
        if (admin.Username == User.Identity?.Name)
        {
            ModelState.AddModelError("Error","Can not remove current login account");
            return View("Index", await _repository.GetAllAsync());
        }
        await _repository.DeleteAsync(admin.Id);
        return RedirectToAction("Index", await _repository.GetAllAsync());
    }
}