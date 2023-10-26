using Internals.Models;
using Internals.Models.Enum;

namespace Internals.ViewModels;

public class FilterModel
{
    public List<int>? CategoryFilter { get; set; }
    public List<string>? ColorFilter { get; set; }
    public List<string>? RamFilter { get; set; }
    public List<string>? StorageFilter { get; set; }
    public List<string>? BrandFilter { get; set; }
    
    public string? KeySearch { get; set; }
    public double? MinRangePrice { get; set; }
    public double? MaxRangePrice { get; set; }
    public int PageNum { get; set; }
    public int Quantity { get; set; }

    public bool IsEmpty()
    {
        return CategoryFilter.Count==0 && 
               ColorFilter.Count==0 &&
               RamFilter.Count==0 &&
               StorageFilter.Count==0 &&
               BrandFilter.Count==0;
    }

    public List<BrandPhone> GetBrandPhones()
    {
        return BrandFilter!.Select(Enum.Parse<BrandPhone>).ToList();
    }
    public List<Color> GetColors()
    {
        return ColorFilter!.Select(Enum.Parse<Color>).ToList();
    }
    public List<RAM> GetRams()
    {
        return RamFilter!.Select(Enum.Parse<RAM>).ToList();
    }
    public List<Storage> GetStorages()
    {
        return StorageFilter!.Select(Enum.Parse<Storage>).ToList();
    }

    public bool CheckKeySearch(Enum eEnum)
    {
        return eEnum.ToString().Contains(KeySearch);
    }
}