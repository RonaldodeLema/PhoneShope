using Internals.Database;
using Internals.Models;
using Internals.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Internals.Repository;

public class PhoneDetailRepository : IRepository<PhoneDetails, int>, IPhoneDetailRepository
{
    private readonly DbPhoneStoreContext _context;

    public PhoneDetailRepository(DbPhoneStoreContext context)
    {
        _context = context;
    }

    public async Task<List<PhoneDetails>> GetAllAsync()
    {
        return await _context.PhoneDetails.Include(c => c.Phone).ToListAsync();
    }

    public async Task<PhoneDetails> GetByIdAsync(int id)
    {
        return (await _context.PhoneDetails
            .Include(pd => pd.Phone)
            .FirstOrDefaultAsync(pd => pd.Id == id))!;
    }

    public async Task<PhoneDetails> AddAsync(PhoneDetails category)
    {
        var add = _context.Add(category);
        await _context.SaveChangesAsync();
        return add.Entity;
    }

    public async Task<PhoneDetails> UpdateAsync(PhoneDetails category)
    {
        var up = _context.Update(category);
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

    public async Task<List<PhoneDetails>> GetByFilter(FilterModel filterModel)
    {
        var query = _context.PhoneDetails.Include(p => p.Phone).AsQueryable();

        if (filterModel.BrandFilter != null && filterModel.BrandFilter.Count != 0)
        {
            query = query.Where(p => filterModel.GetBrandPhones().Contains(p.Phone.Brand));
        }

        if (filterModel.ColorFilter != null && filterModel.ColorFilter.Count != 0)
        {
            query = query.Where(p => filterModel.GetColors().Contains(p.Color));
        }

        if (filterModel.RamFilter != null && filterModel.RamFilter.Count != 0)
        {
            query = query.Where(p => filterModel.GetRams().Contains(p.RAM));
        }

        if (filterModel.StorageFilter != null && filterModel.StorageFilter.Count != 0)
        {
            query = query.Where(p => filterModel.GetStorages().Contains(p.Storage));
        }

        if (filterModel.CategoryFilter != null && filterModel.CategoryFilter.Count != 0)
        {
            query = query.Where(p => filterModel.CategoryFilter.Contains(p.Phone.CategoryId));
        }

        if (filterModel.MinRangePrice.HasValue)
        {
            query = query.Where(p => p.Price >= filterModel.MinRangePrice.Value);
        }

        if (filterModel.MaxRangePrice.HasValue)
        {
            query = query.Where(p => p.Price <= filterModel.MaxRangePrice.Value);
        }

        if (!string.IsNullOrEmpty(filterModel.KeySearch))
        {
            query = query.Where(p => EF.Functions.Like(p.Phone.Name, $"%{filterModel.KeySearch}%"));
        }

        return await query.ToListAsync();
    }
}