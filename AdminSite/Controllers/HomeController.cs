using System.Diagnostics;
using Internals.Models;
using Internals.Repository;
using Internals.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminSite.Controllers;

[Authorize]
public class HomeController : Controller
{

    private readonly IOrderRepository _orderRepository;

    public HomeController(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IActionResult> Index()
    {
        var statistic = new Statistics()
        {
            GetMonthly = await _orderRepository.GetEarningMonthly(DateTime.Now.Month),
            GetAnnually = await _orderRepository.GetEarningAnnually(DateTime.Now.Year),
            GetOrderPending = await _orderRepository.GetAllOrderPending(),
        };
        return View(statistic);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}