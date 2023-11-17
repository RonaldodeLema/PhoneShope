using Internals.Models;

namespace Internals.Services;

public interface IPromotionService
{
    Task<List<Promotion>> GetsByUserId(int id);
}