using Internals.Models.Enum;

namespace Internals.Models;

public class Payment
{
    public int Id { get; set; }
    public string Owner;
    public Method Method;
    public string NumberCard { get; set; }
    public string QRCode { get; set; }
}