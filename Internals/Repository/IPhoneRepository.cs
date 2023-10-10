using Internals.Models;

namespace Internals.Repository;

public interface IPhoneRepository
{
    Task<List<Phone>> GetAllByCategoryId(int? categoryId);
}