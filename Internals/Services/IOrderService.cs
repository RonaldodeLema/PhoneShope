using Internals.Models;

namespace Internals.Services;

public interface IOrderService
{
    public Task<List<Order>> GetAll();
    public Task<Order> AddAsync(Order order);
    public Task<Order> GetById(int id);
    public Task<OrderItem> AddOrderItemsAsync(OrderItem orderItem);
    public Task<List<Order>> GetOrderByUserId(int id);
}