using Internals.Models;

namespace Internals.Repository;

public interface IImageRepository
{
    Image AddImage(Image image);
    void DeleteImage(string publicId);
    Image FindByImageUrl(string url);
}