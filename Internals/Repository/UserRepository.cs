using Internals.Database;
using Internals.Models;
using Microsoft.EntityFrameworkCore;

namespace Internals.Repository;

public class UserRepository:IRepository<User,int>, IUserRepository
{
    private readonly DbPhoneStoreContext _context;

    public UserRepository(DbPhoneStoreContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User> AddAsync(User category)
    {
        var add = _context.Add(category);
        await _context.SaveChangesAsync();
        return add.Entity;
    }

    public async Task<User> UpdateAsync(User category)
    {
        var up= _context.Update(category);
        await _context.SaveChangesAsync();
        return up.Entity;
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _context.Users.FindAsync(id);
        if (category != null)
        {
            _context.Users.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}