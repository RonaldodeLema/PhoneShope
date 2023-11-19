using Internals.Models;
using Internals.ViewModels;

namespace Internals.Repository;

public interface IUserRepository
{
    Task<User?> Login(UserLogin userLogin);
    Task<User> Register(UserRegister userRegister);
    Task<User?> FindByUsername(string username);
}