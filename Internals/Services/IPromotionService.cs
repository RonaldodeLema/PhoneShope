using Internals.Models;

namespace Internals.Services;

public interface IPromotionService
{
    Task<List<Promotion>> GetsByUserId(int id);
    Task<Promotion> UpdatePromotion(Promotion promotion);
    Task<Promotion> FindById(int id);
}