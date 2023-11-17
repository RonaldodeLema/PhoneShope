using Internals.Repository;
using Microsoft.AspNetCore.Authorization;

namespace AdminSite.Policies;

public class ManageModelHandler : AuthorizationHandler<ManageModelRequirement>
{

    private readonly IRoleRepository _roleRepository;

    public ManageModelHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        ManageModelRequirement requirement)
    {
        var user = context.User.Identity.Name;
        var roleClaims =  await _roleRepository.GetRoleClaimsByUname(user);
        if (!roleClaims.Contains(requirement.NamePolicy))
        {
            context.Fail();
        }
        else
        {
            context.Succeed(requirement);
        }
    }
}