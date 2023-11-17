using Internals.Models;
using Internals.Models.Enum;

namespace Internals.ViewModels;

public class ReqOrderModel
{
    public string? Note { get; set; }
    public Method Method { get; set; }
    public string? PayCode { get; set; }
    public Promotion Promotion { get; set; }
}