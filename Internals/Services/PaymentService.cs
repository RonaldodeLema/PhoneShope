using Internals.Models;
using Internals.Repository;

namespace Internals.Services;

public class PaymentService: IPaymentService
{
    private readonly IRepository<Payment, int> _repository;

    public PaymentService(IRepository<Payment, int> repository)
    {
        _repository = repository;
    }
    public Task<List<Payment>> GetAll()
    {
        return _repository.GetAllAsync();
    }
}