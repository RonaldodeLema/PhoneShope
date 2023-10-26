using Internals.Models;
using Internals.ViewModels;

namespace Internals.Repository;

public interface IPhoneDetailRepository
{
    Task<List<PhoneDetails>> GetAllByPhoneId(int? categoryId);
    Task<List<PhoneDetails>> GetByFilter(FilterModel filterModel);

}