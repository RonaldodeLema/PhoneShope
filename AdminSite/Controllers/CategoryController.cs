using Internals.Models;
using Internals.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace AdminSite.Controllers;


[Authorize(Roles = "RootAdmin, Admin")]
public class CategoryController : Controller
{
    private readonly IRepository<Category,int> _repository;

    public CategoryController(IRepository<Category,int> repository)
    {
       
        _repository = repository;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _repository.GetAllAsync();
        return View(categories);
    }
    

    [HttpPost]
    public async Task<JsonResult> Create([FromBody] Category category)
    {
        category.SetDateTime();
        category.SetActionBy(User.Identity?.Name);
        var newCategory = await _repository.AddAsync(category);
        return new JsonResult(newCategory.ToJson());
    }

    
    [HttpPost]
    public async Task<JsonResult> Edit([FromBody] Category category)
    {
        category.SetUpdateDate();
        category.SetUpdateBy(User.Identity?.Name);
        var newCategory = await _repository.UpdateAsync(category);
        return new JsonResult(newCategory.ToJson());
    }

    public async Task<JsonResult> Delete(int id)
    {
        var category = await _repository.GetByIdAsync(id);
        await _repository.DeleteAsync(category.Id);
        return  new JsonResult(category.ToJson());
    }
}
