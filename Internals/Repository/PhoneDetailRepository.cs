using Internals.Database;
using Internals.Models;
using Microsoft.EntityFrameworkCore;

namespace Internals.Repository;

public class PhoneDetailRepository:IRepository<PhoneDetails,int>,IPhoneDetailRepository
{
    private readonly DbPhoneStoreContext _context;

    public PhoneDetailRepository(DbPhoneStoreContext context)
    {
        _context = context;
    }

    public async Task<List<PhoneDetails>> GetAllAsync()
    {
        return await _context.PhoneDetails.ToListAsync();
    }

    public async Task<PhoneDetails> GetByIdAsync(int id)
    {
        return await _context.PhoneDetails.FindAsync(id);
    }

    public async Task<PhoneDetails> AddAsync(PhoneDetails category)
    {
        var add = _context.Add(category);
        await _context.SaveChangesAsync();
        return add.Entity;
    }

    public async Task<PhoneDetails> UpdateAsync(PhoneDetails category)
    {
        var up= _context.Update(category);
        await _context.SaveChangesAsync();
        return up.Entity;
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _context.PhoneDetails.FindAsync(id);
        if (category != null)
        {
            _context.PhoneDetails.Remove(category);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<PhoneDetails>> GetAllByPhoneId(int? phoneId)
    {
        return await _context.PhoneDetails.Where(p => p.PhoneId == phoneId).ToListAsync();
    }
}