using Internals.Models;
using Internals.Repository;
using Internals.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminSite.Controllers;
[Authorize("Manage_Order")]
public class OrderController : Controller
{
    private readonly IOrderRepository _orderRepository;
    private readonly IRepository<Order,int> _repository;

    public OrderController(IOrderRepository orderRepository,IRepository<Order,int> repository)
    {
        _orderRepository = orderRepository;
        _repository = repository;
    }
    // GET
    public async Task<IActionResult> Index()
    {
        var orders = await _repository.GetAllAsync();
        return View(orders);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([Bind("Id,Status")] StatusEdit statusEdit)
    {
        var order = _repository.GetByIdAsync(statusEdit.Id).Result;
        order.Status = statusEdit.Status;
        order.SetUpdateDate();
        await _repository.UpdateAsync(order);
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Details(int id)
    {
        var phone = await _repository.GetByIdAsync(id);

        return View(phone);
    }
}