using Internals.Database;
using Internals.Models;
using Microsoft.EntityFrameworkCore;

namespace Internals.Repository;

public class PromotionRepository: IRepository<Promotion,int>
{
    private readonly DbPhoneStoreContext _context;

    public PromotionRepository(DbPhoneStoreContext context)
    {
        _context = context;
    }

    public async Task<List<Promotion>> GetAllAsync()
    {
        return await _context.Promotions.ToListAsync();
    }

    public async Task<Promotion> GetByIdAsync(int id)
    {
        return await _context.Promotions.FindAsync(id);
    }

    public async Task<Promotion> AddAsync(Promotion promotion)
    {
        var add = _context.Add(promotion);
        await _context.SaveChangesAsync();
        return add.Entity;
    }

    public async Task<Promotion> UpdateAsync(Promotion promotion)
    {
        var up= _context.Update(promotion);
        await _context.SaveChangesAsync();
        return up.Entity;
    }

    public async Task DeleteAsync(int id)
    {
        var promotion = await _context.Promotions.FindAsync(id);
        if (promotion != null)
        {
            _context.Promotions.Remove(promotion);
            await _context.SaveChangesAsync();
        }
    }
}