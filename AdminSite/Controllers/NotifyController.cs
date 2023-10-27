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
                "cDiq3qHMrL9SGBejfXeE64:APA91bFCtaXqqutifrMVhushFJ89qUMI1K2udeuyilb42xJ-cUpPyo2QfqslAEc81k8HSgFwYPcI--57OVnBluBDj6ybqACXhsoqefWbQCjkXyFZTRPBAxjX_yU367DPJ5UHmfvOdS9U"
            },
        };
        var result = await _fcmService.FirebaseMessaging.SendMulticastAsync(message);
        Console.Write(result);
        return View();
    }
}