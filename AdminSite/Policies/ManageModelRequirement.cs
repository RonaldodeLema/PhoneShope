using Internals.Models.Enum;
using Microsoft.AspNetCore.Authorization;

namespace AdminSite.Policies;

public class ManageModelRequirement: IAuthorizationRequirement
{
    public Manage_Model NamePolicy;

    public ManageModelRequirement(Manage_Model namePolicy)
    {
        NamePolicy = namePolicy;
    }
}