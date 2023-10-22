using Internals.Models;
using Internals.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminSite.Controllers;

[Authorize("Manage_Promotion")]
public class PromotionController : Controller
{
    private readonly IRepository<Promotion,int> _repository;

    public PromotionController(IRepository<Promotion,int> repository)
    {
        _repository = repository;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var promotions = await _repository.GetAllAsync();
        return View(promotions);
    }
}