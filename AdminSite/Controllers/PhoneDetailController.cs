using Internals.Models;
using Internals.Repository;
using Internals.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol;

namespace AdminSite.Controllers;

[Authorize("Manage_Phone_Detail")]
public class PhoneDetailController : Controller
{
    private readonly IRepository<PhoneDetails, int> _repository;
    private readonly IPhoneDetailRepository _phoneDetailRepository;
    private readonly IRepository<Phone, int> _phoneRepository;
    private readonly IRepository<Category, int> _categoryRepository;

    public PhoneDetailController(IPhoneDetailRepository phoneDetailRepository, IRepository<PhoneDetails, int> repository,
        IRepository<Category, int> categoryRepository, IRepository<Phone,int> phoneRepository)
    {
        _phoneDetailRepository = phoneDetailRepository;
        _repository = repository;
        _categoryRepository = categoryRepository;
        _phoneRepository = phoneRepository;
    }

    public async Task<IActionResult> Index()
    {
        var phones = await _phoneRepository.GetAllAsync();
        var selectList = new SelectList(phones, "Id", "Name");
        ViewData["Phones"] = selectList;
        return View();
    }
    public async Task<PartialViewResult> GetPhoneDetail(int? id)
    {
        if (id != null) return PartialView(await _phoneDetailRepository.GetAllByPhoneId(id));
        await _repository.GetAllAsync();
        return PartialView(await _repository.GetAllAsync());
    }
    public async Task<IActionResult> Create()
    {
        var phones = await _phoneRepository.GetAllAsync();
        var selectList = new SelectList(phones, "Id", "Name");
        ViewData["Phones"] = selectList;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("PhoneId,Size,Color,RAM,Storage,Quantity,Price")] PhoneDetailsCreate phoneDetailsCreate)
    {
        if (!ModelState.IsValid) return View(phoneDetailsCreate);
        var phoneDetails = phoneDetailsCreate.ConvertToPhoneDetails();
        phoneDetails.SetDateTime();
        phoneDetails.SetActionBy(User.Identity.Name);
        await _repository.AddAsync(phoneDetails);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var phones = await _phoneRepository.GetAllAsync();
        var selectList = new SelectList(phones, "Id", "Name");
        ViewData["Phones"] = selectList;
        var phoneDetails = _repository.GetByIdAsync(id).Result;
        return View(phoneDetails);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,PhoneId,Size,Color,RAM,Storage,Quantity,Price")] PhoneDetails phoneDetails)
    {
        var oldPhoneDetails = _repository.GetByIdAsync(phoneDetails.Id).Result;
        oldPhoneDetails.PhoneId = phoneDetails.PhoneId;
        oldPhoneDetails.Size = phoneDetails.Size;
        oldPhoneDetails.Color = phoneDetails.Color;
        oldPhoneDetails.RAM = phoneDetails.RAM;
        oldPhoneDetails.Storage = phoneDetails.Storage;
        oldPhoneDetails.Quantity = phoneDetails.Quantity;
        oldPhoneDetails.Price = phoneDetails.Price;
        oldPhoneDetails.SetUpdateBy(User.Identity.Name);
        oldPhoneDetails.SetUpdateDate();
        await _repository.UpdateAsync(oldPhoneDetails);
        var phones = await _phoneRepository.GetAllAsync();
        var selectList = new SelectList(phones, "Id", "Name");
        ViewData["Phones"] = selectList;
        return RedirectToAction(nameof(Index));
    }


    public async Task<JsonResult> Delete(int id)
    {
        var phone = await _repository.GetByIdAsync(id);
        await _repository.DeleteAsync(phone.Id);
        return new JsonResult(phone.ToJson());
    }

    public async Task<IActionResult> Details(int id)
    {
        var phone = await _repository.GetByIdAsync(id);

        return View(phone);
    }
}