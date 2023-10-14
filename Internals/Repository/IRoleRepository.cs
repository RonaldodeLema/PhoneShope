using Internals.Models;
using Internals.Models.Enum;

namespace Internals.Repository;

public interface IRoleRepository
{
    Task<List<ManageModel>> GetRoleClaimsByUname(string username);
    Task<List<ManageModel>> GetRoleClaimsByRoleId(int id);
    Task<RoleClaim> AddRoleClaimAsync(RoleClaim roleClaim);
    Task RemoveAllByRoleId(int id);
}