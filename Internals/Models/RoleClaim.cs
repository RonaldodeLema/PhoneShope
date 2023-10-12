using Internals.Models.Enum;

namespace Internals.Models;

public class RoleClaim
{
    public int Id { get; set; }
    public int RoleId { get; set; }
    public Manage_Model ManageModel { get; set; }

    public virtual Role Role { get; set; }
}