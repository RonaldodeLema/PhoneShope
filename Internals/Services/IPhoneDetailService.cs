using Internals.Models;
using Internals.ViewModels;

namespace Internals.Services;

public interface IPhoneDetailService
{
    Task<List<PhoneDetails>> GetAll();
    Task<List<Category>> GetAllCategory();
    Task<List<PhoneDetails>> GetByFilter(FilterModel filterModel);
    Task<PhoneDetails> GetById(int id);

}