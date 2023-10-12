using Internals.Models;
using Internals.Models.Enum;

namespace Internals.ViewModels;

public class RoleCreate
{
    public string Name { get; set; }
    public ManageModel[] Permissions { get; set; }

    public List<RoleClaim> ConvertToListRoleClaim(Role role)
    {
        return Permissions.Select(permission => new RoleClaim() { RoleId = role.Id,ManageModel = permission }).ToList();
    }
    public Role ConvertToRole()
    {
        return new Role()
        {
            Name = Name
        };
    }
}