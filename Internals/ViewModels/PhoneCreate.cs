using Internals.Models;
using Internals.Models.Enum;

namespace Internals.ViewModels;

public class PhoneCreate
{
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string? Image { get; set; }
    public BrandPhone Brand { get; set; }

    public Phone ConvertToPhone()
    {
        return new Phone()
        {
            CategoryId = CategoryId,
            Name = Name,
            Description = Description,
            Brand = Brand
        };
    }
}