using Internals.Database;
using Internals.Models;
using Internals.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace Internals.Repository;

public class RoleRepository : IRepository<Role, int>, IRoleRepository
{
    private readonly DbPhoneStoreContext _context;

    public RoleRepository(DbPhoneStoreContext context)
    {
        _context = context;
    }

    public async Task<List<Role>> GetAllAsync()
    {
        return await _context.Roles.Include(c => c.RoleClaims).ToListAsync();
    }

    public async Task<Role> GetByIdAsync(int id)
    {
        return await _context.Roles.FindAsync(id);
    }

    public async Task<Role> AddAsync(Role role)
    {
        var add = _context.Add(role);
        await _context.SaveChangesAsync();
        return add.Entity;
    }

    public async Task<Role> UpdateAsync(Role role)
    {
        var up = _context.Update(role);
        await _context.SaveChangesAsync();
        return up.Entity;
    }

    public async Task DeleteAsync(int id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role != null)
        {
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Manage_Model>> GetRoleClaimsByUname(string username)
    {
        var user =  _context.Admins.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
        {
            throw new Exception($"Admin '{username}' does not exist.");
        }

        var claims = await _context.RoleClaims
            .Where(rc => rc.RoleId == user.Result.RoleId)
            .Select(rc=>rc.ManageModel)
            .ToListAsync();
        return claims;
    }

    public async Task<RoleClaim> AddRoleClaimAsync(RoleClaim roleClaim)
    {
        var add = _context.RoleClaims.Add(roleClaim);
        await _context.SaveChangesAsync();
        return add.Entity;
    }
}