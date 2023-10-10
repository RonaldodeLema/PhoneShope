using Internals.Models;

namespace Internals.Repository;

public interface IPhoneDetailRepository
{
    Task<List<PhoneDetails>> GetAllByPhoneId(int? categoryId);
}