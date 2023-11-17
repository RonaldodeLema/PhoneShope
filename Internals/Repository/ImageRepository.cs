using Internals.Database;
using Internals.Models;

namespace Internals.Repository;

public class ImageRepository:IImageRepository
{
    private readonly DbPhoneStoreContext _context;

    public ImageRepository(DbPhoneStoreContext context)
    {
        _context = context;
    }
    
    public Image AddImage(Image image)
    {
        var add = _context.Add(image);
         _context.SaveChanges();
        return add.Entity;
    }

    public void DeleteImage(string publicId)
    {
        var image =  _context.Images.Find(publicId);
        if (image == null) return;
        _context.Images.Remove(image);
        _context.SaveChanges();
    }

    public Image? FindByImageUrl(string url)
    {
        if (_context == null)
        {
            return null;
        }

        if (_context.Images == null)
        {
            return null;
        }

        var image = _context.Images.FirstOrDefault(i => i.Url == url);
        return image ?? null;
    }
}