using System.Text.Json.Nodes;
using Internals.Models;
using Internals.Models.Enum;
using Internals.Repository;
using Internals.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol;

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
        return RedirectToAction("Index");
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([Bind("Id,Name,Permissions")] RoleCreate roleCreate)
    {
      
        var task = await _repository.GetByIdAsync(roleCreate.Id);
        // Remove Old RoleClaim
        await _roleRepository.RemoveAllByRoleId(roleCreate.Id);
        //Add new RoleClaim
        var roleClaims = roleCreate.ConvertToListRoleClaim(task);
        foreach (var roleClaim in roleClaims)
        {
            await _roleRepository.AddRoleClaimAsync(roleClaim);
        }
      
        return RedirectToAction("Index");
    }
    public async Task<JsonResult> Details(int id)
    {
        var role = await _repository.GetByIdAsync(id);
        var jsonRole = new JsonObject();
        var listRoleClaim = _roleRepository.GetRoleClaimsByRoleId(role.Id).Result;
        jsonRole.Add("ListRoleClaim",listRoleClaim.ToJson());
        var currRole = new Role()
        {
            Id = role.Id,
            Name = role.Name
        };
        jsonRole.Add("ListAllClaim",Enum.GetValues(typeof(ManageModel)).ToJson());
        jsonRole.Add("Role",currRole.ToJson());
        return new JsonResult(jsonRole);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete([Bind("id")]int id)
    {
        var role = await _repository.GetByIdAsync(id);
        await _repository.DeleteAsync(role.Id);
        return RedirectToAction("Index");
    }
}
