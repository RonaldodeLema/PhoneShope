using Internals.Database;
using Internals.Models;
using Microsoft.EntityFrameworkCore;

namespace Internals.Repository;

public class NotifyRepository: IRepository<Notify, int>
{
    private readonly DbPhoneStoreContext _context;

    public NotifyRepository(DbPhoneStoreContext context)
    {
        _context = context;
    }

    public async Task<List<Notify>> GetAllAsync()
    {
        return await _context.Notifications.ToListAsync();
    }

    public async Task<Notify> GetByIdAsync(int id)
    {
        return await _context.Notifications.FindAsync(id);
    }

    public async Task<Notify> AddAsync(Notify notify)
    {
        var add = _context.Add(notify);
        await _context.SaveChangesAsync();
        return add.Entity;
    }

    public async Task<Notify> UpdateAsync(Notify notify)
    {
        var up= _context.Update(notify);
        await _context.SaveChangesAsync();
        return up.Entity;
    }

    public async Task DeleteAsync(int id)
    {
        var notify = await _context.Notifications.FindAsync(id);
        if (notify != null)
        {
            _context.Notifications.Remove(notify);
            await _context.SaveChangesAsync();
        }
    }
}