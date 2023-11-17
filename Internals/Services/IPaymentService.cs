using Internals.Models;

namespace Internals.Services;

public interface IPaymentService
{
    Task<List<Payment>> GetAll();
}