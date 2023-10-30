using AdminSite.Services;
using FirebaseAdmin.Messaging;
using Internals.Models;
using Internals.Repository;
using Internals.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminSite.Controllers;

[Authorize("Manage_Notify")]
public class NotifyController : Controller
{
    private readonly FcmService _fcmService;
    private readonly IRepository<User, int> _userRepository;
    private readonly IRepository<Notify, int> _repository;

    public NotifyController(FcmService fcmService,  IRepository<Notify,int> repository,
        IRepository<User, int> userRepository)
    {
        _fcmService = fcmService;
        _repository = repository;
        _userRepository = userRepository;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _repository.GetAllAsync());
    }

    [HttpPost]
    public async Task<IActionResult> GetAll(NotifyModel notifyModel)
    {
        var listUsers = await _userRepository.GetAllAsync();
        var listTokens = listUsers.Select(c => c.DeviceToken);
        listTokens = listTokens.Where(l=>l!=null).Distinct();
        
        var notify = notifyModel.ConvertToNotify(HttpContext.User.Identity?.Name);
        var message = new MulticastMessage()
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
        
            Tokens =  listTokens.ToArray()
        };
        await _repository.AddAsync(notify);
        await _fcmService.FirebaseMessaging.SendMulticastAsync(message);
        return RedirectToAction(nameof(Index));
    }
}