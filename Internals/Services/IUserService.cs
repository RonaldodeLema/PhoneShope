using Internals.Models;
using Internals.ViewModels;

namespace Internals.Services;

public interface IUserService
{
    Task<User?> Login(UserLogin userLogin);
    Task<User> Register(UserRegister userRegister);
    Task<User?> FindByUsername(string username);
    Task<User?> FindByEmail(string email);
    Task<User> Update(User user);
}