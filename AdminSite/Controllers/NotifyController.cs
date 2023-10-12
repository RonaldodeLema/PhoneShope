using FirebaseAdmin.Messaging;
using Internals.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminSite.Controllers;
[Authorize("Manage_Notify")]
public class NotifyController : Controller
{

    [HttpPost]
    public async Task<IActionResult> SendMessageAsync([FromBody] Notify notify) 
    {
        var message = new Message()
        {
            Notification = new FirebaseAdmin.Messaging.Notification()
            {
                Title = notify.Title,
                Body = notify.Body,
            },
            Data = new Dictionary<string, string>()
            {
                ["FirstName"] = "John",
                ["LastName"] = "Doe"
            },
            Token = notify.DeviceToken
        };

        var messaging = FirebaseMessaging.DefaultInstance;
        var result = await messaging.SendAsync(message);

        if (!string.IsNullOrEmpty(result))
        {
            // Message was sent successfully
            return Ok("Message sent successfully!");
        }
        else
        {
            // There was an error sending the message
            throw new Exception("Error sending the message.");
        }
    }
}