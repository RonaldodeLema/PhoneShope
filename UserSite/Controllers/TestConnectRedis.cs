using Microsoft.AspNetCore.Mvc;

namespace UserSite.Controllers;

public class TestConnectRedis : Controller
{

    [HttpGet]
    [Route("/write-to-session")]
    public IActionResult TestWriteSession()
    {
        var value = $"Session written at {DateTime.UtcNow.ToString()}";
        HttpContext.Session.SetString("Test", value);

        return Content($"Wrote: {value}");
    }

    [HttpGet]
    [Route("/read-from-session")]
    public IActionResult TestReadSession()
    {
        var value = HttpContext.Session.GetString("Test");

        return Content($"Read: {value}");
    }
}