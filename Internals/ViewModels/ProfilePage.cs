using Internals.Models;

namespace Internals.ViewModels;

public class ProfilePage
{
    public UserUpdate UserUpdate { get; set; }
    public List<Order>? Orders{get; set; }
    public ChangePassword ChangePassword { get; set; }
}