using Internals.Models;
using Internals.Repository;
using Internals.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminSite.Controllers;

[Authorize("Manage_Role")]
public class RoleController : Controller
{
    private readonly IRoleRepository _roleRepository;
    private readonly IRepository<Role,int> _repository;

    public RoleController(IRoleRepository roleRepository,IRepository<Role,int> repository)
    {
        _roleRepository = roleRepository;
        _repository = repository;
    }
    // GET
    public async Task<IActionResult> Index()
    {
        var roles = await _repository.GetAllAsync();
        return View(roles);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Permissions")] RoleCreate roleCreate)
    {
        var newRole = roleCreate.ConvertToRole();
        var task = await _repository.AddAsync(newRole);
        var roleClaims = roleCreate.ConvertToListRoleClaim(task);
        foreach (var roleClaim in roleClaims)
        {
            await _roleRepository.AddRoleClaimAsync(roleClaim);
        }
        var roles = await _repository.GetAllAsync();
        return View("Index",roles);
    }
}
