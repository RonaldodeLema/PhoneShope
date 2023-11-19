using Internals.Database;
using Internals.Models;
using Microsoft.EntityFrameworkCore;

namespace Internals.Repository;

public class PaymentRepository: IRepository<Payment,int>
{
    private readonly DbPhoneStoreContext _context;

    public PaymentRepository(DbPhoneStoreContext context)
    {
        _context = context;
    }

    public async Task<List<Payment>> GetAllAsync()
    {
        return await _context.Payments.ToListAsync();
    }

    public async Task<Payment> GetByIdAsync(int id)
    {
        return await _context.Payments.FindAsync(id);
    }

    public async Task<Payment> AddAsync(Payment payment)
    {
        var add = _context.Add(payment);
        await _context.SaveChangesAsync();
        return add.Entity;
    }

    public async Task<Payment> UpdateAsync(Payment payment)
    {
        var up= _context.Update(payment);
        await _context.SaveChangesAsync();
        return up.Entity;
    }

    public async Task DeleteAsync(int id)
    {
        var payment = await _context.Payments.FindAsync(id);
        if (payment != null)
        {
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
        }
    }
}