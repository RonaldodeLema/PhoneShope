using Internals.Database;
using Internals.Models;
using Internals.ViewModels;
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

    public async Task<User> AddAsync(User user)
    {
        var add = _context.Add(user);
        await _context.SaveChangesAsync();
        return add.Entity;
    }

    public async Task<User> UpdateAsync(User user)
    {
        var up= _context.Update(user);
        await _context.SaveChangesAsync();
        return up.Entity;
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<User?> Login(UserLogin userLogin)
    {
        userLogin.HashPassword();
        var user = await _context.Users.FirstOrDefaultAsync(u =>
            u.Username == userLogin.Username && u.Password == userLogin.Password);
        if (user == null || user.IsBlocked)
        {
            return null;
        }
        return user;
    }

    public async Task<User> Register(UserRegister userRegister)
    {
        var add = _context.Add(userRegister.ConvertToUser());
        await _context.SaveChangesAsync();
        return add.Entity;
    }

    public async Task<User?> FindByUsername(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u =>
            u.Username == username);
        return user;
    }
}