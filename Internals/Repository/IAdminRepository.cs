using Internals.ViewModels;
using Internals.Models;

namespace Internals.Repository;

public interface IAdminRepository
{
    Task<Admin?>? Login(UserLogin userLogin);
    Task<Admin?>? FindByUsername(string userName);

}