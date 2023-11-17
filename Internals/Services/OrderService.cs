using Internals.Models;
using Internals.Repository;

namespace Internals.Services;

public class OrderService: IOrderService
{
    private readonly OrderRepository _repository;
    public OrderService(OrderRepository repository)
    {
        _repository = repository;
    }
    public Task<List<Order>> GetAll()
    {
        return _repository.GetAllAsync();
    }

    public async Task<Order> AddAsync(Order order)
    {
        return await _repository.AddAsync(order);
    }
    public async Task<OrderItem> AddOrderItemsAsync(OrderItem orderItem)
    {
        return await _repository.AddOrderItemsAsync(orderItem);
    }
}