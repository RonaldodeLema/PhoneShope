using AdminSite.Services;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminSite.Controllers;

[Authorize("Manage_Notify")]
public class NotifyController : Controller
{
    private readonly FcmService _fcmService;

    public NotifyController(FcmService fcmService)
    {
        _fcmService = fcmService;
    }

    public async Task<IActionResult> Index()
    {
        var message = new MulticastMessage()
        {
            Webpush = new WebpushConfig()
            {
                Data = new Dictionary<string, string>()
                {
                    ["FirstName"] = "John",
                    ["LastName"] = "Doe"
                },
            },

            Tokens = new[]
            {
                "eh5de4GSo67fIneQoHF62Y:APA91bGIFAvHrfZa6oANuZzZR6Vfty2B7z0ATxn4YFndcFi4auEdR-WlPb6IEjWq5zj_3Dd7iFQuxzHdfp1MtuQvfgKGkDai-NvorIOevJup-oNcfOx2zXG3xdMKbe0tND3P0ZI3iPTN"
            },
        };
        var result = await _fcmService.FirebaseMessaging.SendMulticastAsync(message);
        Console.Write(result);
        return View();
    }
}