using Internals.Database;
using Internals.Models;
using Microsoft.EntityFrameworkCore;

namespace Internals.Repository;

public class CategoryRepository : IRepository<Category,int>
{
    private readonly DbPhoneStoreContext _context;

    public CategoryRepository(DbPhoneStoreContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<Category> GetByIdAsync(int id)
    {
        return await _context.Categories.FindAsync(id);
    }

    public async Task<Category> AddAsync(Category category)
    {
        var add = _context.Add(category);
        await _context.SaveChangesAsync();
        return add.Entity;
    }

    public async Task<Category> UpdateAsync(Category category)
    {
       var up= _context.Update(category);
        await _context.SaveChangesAsync();
        return up.Entity;
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
