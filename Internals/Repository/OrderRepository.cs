using Internals.Database;
using Internals.Models;
using Internals.Models.Enum;
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

    public async Task<double> GetEarningMonthly(int month)
    {
        var orders = await _context.OrderItems
            .Include(item => item.Order)
            .Include(item => item.PhoneDetails)
            .Where(item => item.Order.CreatedAt.Month == month && item.Order.Status == Status.Success)
            .ToListAsync();

        var monthlyEarnings = orders.Sum(order => order.TotalItems());

        return monthlyEarnings;
    }

    public async Task<double> GetEarningAnnually(int year)
    {
        var orders = await _context.OrderItems
            .Include(item => item.Order)
            .Include(item => item.PhoneDetails)
            .Where(item => item.Order.CreatedAt.Year == year && item.Order.Status == Status.Success)
            .ToListAsync();

        var annualEarnings = orders.Sum(order => order.TotalItems());

        return annualEarnings;
    }


    public async Task<int> GetAllOrderPending()
    {
        return await _context.Orders.CountAsync(a => a.Status == Status.Depending);
    }
}