using Internals.Models;

namespace Internals.Services;

public interface IOrderService
{
    public Task<List<Order>> GetAll();
    public Task<Order> AddAsync(Order order);
    public Task<OrderItem> AddOrderItemsAsync(OrderItem orderItem);

}