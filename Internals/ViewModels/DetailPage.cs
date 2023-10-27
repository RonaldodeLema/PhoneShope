using Internals.Models;

namespace Internals.ViewModels;

public class DetailPage
{
    public PhoneDetails PhoneDetails { get; set; }
    public List<PhoneDetails> BestSellers { get; set; }
}