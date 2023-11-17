using Internals.Models;
using Internals.Repository;
using Internals.ViewModels;

namespace Internals.Services;

public class PhoneDetailService:IPhoneDetailService
{
    private readonly IRepository<Category, int> _categoryRepository;
    private readonly IRepository<PhoneDetails, int> _phoneDetailsRepository;
    private readonly IPhoneDetailRepository _repository;

    public PhoneDetailService(IRepository<Category, int> categoryRepository,
        IRepository<PhoneDetails, int> phoneDetailsRepository,
        IPhoneDetailRepository repository
    )
    {
        _categoryRepository = categoryRepository;
        _phoneDetailsRepository = phoneDetailsRepository;
        _repository = repository;
    }

    public async Task<List<PhoneDetails>> GetAll()
    {
        return await _phoneDetailsRepository.GetAllAsync();
    }

    public async Task<List<Category>> GetAllCategory()
    {
        return await _categoryRepository.GetAllAsync();
    }

    public async Task<List<PhoneDetails>> GetByFilter(FilterModel filterModel)
    {
        return await _repository.GetByFilter(filterModel);
    }

    public async Task<PhoneDetails> GetById(int id)
    {
        return await _phoneDetailsRepository.GetByIdAsync(id);
    }

    public async Task<PhoneDetails> UpdateAsync(PhoneDetails phoneDetails)
    {
        return await _phoneDetailsRepository.UpdateAsync(phoneDetails);
    }
}