using Internals.Models;

namespace Internals.ViewModels;

public class OrderPageModel
{
    public User? User { get; set; }
    public List<PhoneDetails> PhoneDetailsList { get; set; }
    public List<Payment> Payments { get; set; }
    
    public List<Promotion> Promotions { get; set; }
    public ReqOrderModel? ReqOrderModel { get; set; }
    
    public string OrderCode { get; set; }
}