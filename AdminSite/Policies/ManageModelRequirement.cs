using Internals.Models.Enum;
using Microsoft.AspNetCore.Authorization;

namespace AdminSite.Policies;

public class ManageModelRequirement: IAuthorizationRequirement
{
    public ManageModel NamePolicy;

    public ManageModelRequirement(ManageModel namePolicy)
    {
        NamePolicy = namePolicy;
    }
}