using AdminSite.Services;
using FirebaseAdmin.Messaging;
using Internals.Models;
using Internals.Repository;
using Internals.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminSite.Controllers;

[Authorize("Manage_User")]
public class UserController : Controller
{
    private readonly FcmService _fcmService;

    private readonly IRepository<Notify, int> _notifyRepository;

    private readonly IRepository<User,int> _repository;

    public UserController(IRepository<User,int> repository, FcmService fcmService, IRepository<Notify, int> notifyRepository)
    {
        _repository = repository;
        _fcmService = fcmService;
        _notifyRepository = notifyRepository;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _repository.GetAllAsync();
        return View(users);
    }
    [HttpPost]
    public async Task<IActionResult> SendMessage(NotifyModel notifyModel)
    {
        var user = await _repository.GetByIdAsync(notifyModel.UserId);
        if (user.DeviceToken == null)
        {
            ModelState.AddModelError("Error","Field empty");
            return RedirectToAction(nameof(Index));
        }
        var notify = notifyModel.ConvertToNotify(HttpContext.User.Identity?.Name);
        var message = new Message()
        {
            Webpush = new WebpushConfig()
            {
                Notification = new WebpushNotification()
                {
                    Title = notify.Title,
                    Body = notify.Body
                },
                Data = notify.Data
            },
        
            Token =  user.DeviceToken
        };
        await _notifyRepository.AddAsync(notify);
        await _fcmService.FirebaseMessaging.SendAsync(message);
        return RedirectToAction(nameof(Index));
    }
}