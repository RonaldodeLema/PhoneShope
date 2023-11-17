using Internals.Models;
using Internals.Repository;

namespace Internals.Services;

public class PromotionService: IPromotionService
{
    private readonly PromotionRepository _repository;

    public PromotionService(PromotionRepository repository)
    {
        _repository = repository;
    }
    public async Task<List<Promotion>> GetsByUserId(int id)
    {
        return await _repository.GetAllByUserId(id);
    }

    public async Task<Promotion> UpdatePromotion(Promotion promotion)
    {
        return await _repository.UpdateAsync(promotion);
    }
    public async Task<Promotion> FindById(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
}