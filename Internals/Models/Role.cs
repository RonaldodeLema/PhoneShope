namespace Internals.Models;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }

    public virtual ICollection<Admin> Admins { get; set; }
    public virtual ICollection<RoleClaim> RoleClaims { get; set; }
}