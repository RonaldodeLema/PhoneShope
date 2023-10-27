using Internals.Models;

namespace Internals.ViewModels;

public class HomePage
{
    public List<PhoneDetails> BestSellers { get; set; }
    public List<PhoneDetails> NewPhones { get; set; }
    
    public string Code { get; set; }

    public List<PhoneDetails> SplitNewPhones(int num)
    {
        return num == 1 ? NewPhones.GetRange(0, NewPhones.Count / 2) : 
            NewPhones.GetRange(NewPhones.Count / 2,NewPhones.Count);
    }
}