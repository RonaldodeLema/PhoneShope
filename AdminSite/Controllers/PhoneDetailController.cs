using AdminSite.Services;
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
    private readonly ImageService _imageService;

    public PhoneDetailController(IPhoneDetailRepository phoneDetailRepository, IRepository<PhoneDetails, int> repository,
        IRepository<Category, int> categoryRepository, IRepository<Phone,int> phoneRepository,
        ImageService imageService)
    {
        _phoneDetailRepository = phoneDetailRepository;
        _repository = repository;
        _categoryRepository = categoryRepository;
        _phoneRepository = phoneRepository;
        _imageService = imageService;
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
    public async Task<IActionResult> Create([Bind("PhoneId,Size,Color,RAM,Storage,Quantity,Price")] PhoneDetailsCreate phoneDetailsCreate,
        [FromForm] IFormFile image)
    {
        if (!ModelState.IsValid) return View(phoneDetailsCreate);
        var imagePhone = _imageService.UploadImage(image);
        var phoneDetails = phoneDetailsCreate.ConvertToPhoneDetails();
        if (imagePhone != null) phoneDetails.Image = imagePhone.Url;
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
    public async Task<IActionResult> Edit(int id, [Bind("Id,PhoneId,Size,Color,RAM,Storage,Quantity,Price")] PhoneDetails phoneDetails,
        [FromForm] IFormFile? image)
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
        if (image != null)
        {
            // remove old image
            var oldImage = _imageService.FindImageByUrl(oldPhoneDetails.Image);
            if (oldImage != null)
            {
                _imageService.DeleteImage(oldImage.PublicId);
            }
            // Add new image
            var newImage = _imageService.UploadImage(image);
            oldPhoneDetails.Image = newImage.Url;
        }
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