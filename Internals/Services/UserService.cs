using Internals.Models;
using Internals.Repository;
using Internals.ViewModels;

namespace Internals.Services;

public class UserService :IUserService
{
    private readonly UserRepository _repository;

    public UserService(UserRepository repository)
    {
        _repository = repository;
    }

    public Task<User?> Login(UserLogin userLogin)
    {
        return _repository.Login(userLogin);
    }

    public Task<User> Register(UserRegister userRegister)
    {
        return _repository.Register(userRegister);
    }

    public async Task<User?> FindByUsername(string username)
    {
        return await _repository.FindByUsername(username);
    }

    public async Task<User> Update(User user)
    {
        return await _repository.UpdateAsync(user);
    }
}