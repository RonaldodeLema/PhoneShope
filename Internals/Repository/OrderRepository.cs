using Internals.Database;
using Internals.Models;
using Microsoft.EntityFrameworkCore;

namespace Internals.Repository;

public class OrderRepository: IRepository<Order,int>, IOrderRepository
{
    private readonly DbPhoneStoreContext _context;
    public OrderRepository( DbPhoneStoreContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetAllAsync()
    {
        return await _context.Orders.Include(a=>a.User)
            .Include(a=>a.OrderItems)!.ThenInclude(a=>a.PhoneDetails)
            .ToListAsync();
    }

    public async Task<Order> GetByIdAsync(int id)
    {
        return (await _context.Orders.Include(a=>a.User)
            .Include(a=>a.OrderItems)!
            .ThenInclude(a=>a.PhoneDetails)
            .ThenInclude(a=>a.Phone)
            .FirstOrDefaultAsync(a=>a.Id==id))!;
    }

    public async Task<Order> AddAsync(Order obj)
    {
        var order =_context.Add(obj);
        await _context.SaveChangesAsync();
        return order.Entity;
    }

    public async Task<Order> UpdateAsync(Order obj)
    {
        var order = _context.Update(obj);
        await _context.SaveChangesAsync();
        return order.Entity;
    }

    public async Task DeleteAsync(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order != null)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}