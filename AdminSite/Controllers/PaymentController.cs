using Internals.Models;
using Internals.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AdminSite.Controllers;

public class PaymentController: Controller
{
    private readonly IRepository<Payment,int> _repository;

    public PaymentController(IRepository<Payment,int> repository)
    {
       
        _repository = repository;
    }

    public async Task<IActionResult> Index()
    {
        var payments = await _repository.GetAllAsync();
        Console.Write(payments.Find(p=>p.Id==1).Owner);
        var test = 1;
        return View(payments);
    }
}