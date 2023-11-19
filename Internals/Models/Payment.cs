using Internals.Models.Enum;

namespace Internals.Models;

public class Payment
{
    public int Id { get; set; }
    public string Owner { get; set; }
    public Method Method { get; set; }
    public string NumberCard { get; set; }
    public string QRCode { get; set; }
}