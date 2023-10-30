using Internals.Models;

namespace Internals.ViewModels;

public class NotifyModel
{
    public int? UserId { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }

    public Notify ConvertToNotify(string? username)
    {
        var dict = new Dictionary<string, string> { { Key, Value } };
        var notify = new Notify()
        {
            Title = Title,
            Body = Body,
            Data = dict
        };
        notify.SetActionBy(username);
        notify.SetDateTime();
        return notify;
    }
}