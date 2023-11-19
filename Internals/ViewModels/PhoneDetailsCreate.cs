using Internals.Models;
using Internals.Models.Enum;

namespace Internals.ViewModels;

public class PhoneDetailsCreate
{
    public int PhoneId { get; set; }
    public Size Size { get; set; }
    public Color Color { get; set; }
    public RAM RAM { get; set; }
    public Storage Storage { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }

    public PhoneDetails ConvertToPhoneDetails()
    {
        return new PhoneDetails()
        {
            PhoneId = PhoneId,
            Size = Size,
            Color = Color,
            RAM = RAM,
            Storage = Storage,
            Quantity = Quantity,
            Price = Price
        };
    }
}