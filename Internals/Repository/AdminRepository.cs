using Internals.Database;
using Internals.Models;
using Internals.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Internals.Repository;

public class AdminRepository: IAdminRepository,IRepository<Admin,int>
{
    private readonly DbPhoneStoreContext _context;
    public AdminRepository( DbPhoneStoreContext context)
    {
        _context = context;
    }
    public async Task<Admin?>? Login(UserLogin userLogin)
    {
        var admin = await _context.Admins.FirstOrDefaultAsync(u => u.Username == userLogin.Username);
        if (admin == null) return null;
        return admin.ComparePasswords(userLogin.Password) ? admin : null;
    }

    public async Task<Admin?>? FindByUsername(string userName)
    {
        var admin = await _context.Admins.FirstOrDefaultAsync(u => u.Username == userName);
        return admin;
    }

    public async Task<List<Admin>> GetAllAsync()
    {
        return await _context.Admins.ToListAsync();
    }

    public async Task<Admin> GetByIdAsync(int id)
    {
        return await _context.Admins.FindAsync(id);
    }

    public async Task<Admin> AddAsync(Admin obj)
    {
       var admin =_context.Add(obj);
        await _context.SaveChangesAsync();
        return admin.Entity;
    }

    public async Task<Admin> UpdateAsync(Admin obj)
    {
        var admin = _context.Update(obj);
        await _context.SaveChangesAsync();
        return admin.Entity;
    }

    public async Task DeleteAsync(int id)
    {
        var admin = await _context.Admins.FindAsync(id);
        if (admin != null)
        {
            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();
        }
    }
}