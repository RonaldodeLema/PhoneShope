using Internals.Database;
using Internals.Models;
using Microsoft.EntityFrameworkCore;

namespace Internals.Repository;

public class PhoneRepository:IRepository<Phone,int>,IPhoneRepository
{
    private readonly DbPhoneStoreContext _context;

    public PhoneRepository(DbPhoneStoreContext context)
    {
        _context = context;
    }

    public async Task<List<Phone>> GetAllAsync()
    {
        return await _context.Phones.ToListAsync();
    }

    public Task<Phone> GetByIdAsync(int id)
    {
        return Task.FromResult(_context.Phones.Include(phone => phone.Category).FirstOrDefault(p => p.Id == id)!);
    }

    public async Task<Phone> AddAsync(Phone phone)
    {
        var add = _context.Add(phone);
        await _context.SaveChangesAsync();
        return add.Entity;
    }

    public async Task<Phone> UpdateAsync(Phone phone)
    {
        var up= _context.Update(phone);
        await _context.SaveChangesAsync();
        return up.Entity;
    }

    public async Task DeleteAsync(int id)
    {
        var phone = await _context.Phones.FindAsync(id);
        if (phone != null)
        {
            _context.Phones.Remove(phone);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Phone>> GetAllByCategoryId(int? categoryId)
    {
        return await _context.Phones.Where(p => p.CategoryId == categoryId).ToListAsync();
    }
}