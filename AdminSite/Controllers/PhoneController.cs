using AdminSite.Manager;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Internals.Models;
using Internals.Repository;
using Internals.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol;

namespace AdminSite.Controllers;

[Authorize(Roles = "RootAdmin, Admin")]
public class PhoneController : Controller
{
    private readonly IRepository<Phone, int> _repository;
    private readonly IPhoneRepository _phoneRepository;
    private readonly IRepository<Category, int> _categoryRepository;
    private readonly ImageService _imageService;


    public PhoneController(IPhoneRepository phoneRepository, IRepository<Phone, int> repository,
        IRepository<Category, int> categoryRepository,ImageService imageService)
    {
        _phoneRepository = phoneRepository;
        _repository = repository;
        _categoryRepository = categoryRepository;
        _imageService = imageService;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _categoryRepository.GetAllAsync();
        var selectList = new SelectList(categories, "Id", "Name");
        ViewData["Categories"] = selectList;
        return View();
    }
    public async Task<PartialViewResult> GetPhone(int? id)
    {
        if (id != null) return PartialView(await _phoneRepository.GetAllByCategoryId(id));
        await _repository.GetAllAsync();
        return PartialView(await _repository.GetAllAsync());
    }
    public async Task<IActionResult> Create()
    {
        var categories = await _categoryRepository.GetAllAsync();
        var selectList = new SelectList(categories, "Id", "Name");
        ViewData["Categories"] = selectList;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Description,CategoryId,Brand")] PhoneCreate createPhone,
        [FromForm] IFormFile image)
    {
        if (!ModelState.IsValid) return View(createPhone);
        var imagePhone = _imageService.UploadImage(image);
        var phone = createPhone.ConvertToPhone();
        phone.Image = imagePhone.Url;
        phone.SetDateTime();
        phone.SetActionBy(User.Identity.Name);
        await _repository.AddAsync(phone);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var phone = await _repository.GetByIdAsync(id);
        var categories = await _categoryRepository.GetAllAsync();
        var selectList = new SelectList(categories, "Id", "Name");
        ViewData["Categories"] = selectList;
        return View(phone);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CategoryId,Brand")] Phone phone,
        [FromForm] IFormFile? image)
    {
        var task = _repository.GetByIdAsync(id);
        var currentPhone = task.Result;
        currentPhone.Name = phone.Name;
        currentPhone.Description = phone.Description;
        currentPhone.CategoryId = phone.CategoryId;
        currentPhone.Brand = phone.Brand;
        currentPhone.SetUpdateBy(User.Identity.Name);
        currentPhone.SetUpdateDate();
        if (image != null)
        {
            // remove old image
            var oldImage = _imageService.FindImageByUrl(currentPhone.Image);
            _imageService.DeleteImage(oldImage.PublicId);
            // Add new image
            var newImage = _imageService.UploadImage(image);
            currentPhone.Image = newImage.Url;
        }

        await _repository.UpdateAsync(currentPhone);
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