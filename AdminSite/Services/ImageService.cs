using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Internals.Models;
using Internals.Repository;

namespace AdminSite.Services;

public class ImageService
{
    private IConfiguration Configuration { get; }
    private CloudinarySettings _cloudinarySettings;
    private Cloudinary _cloudinary;
    private IImageRepository _imageRepository;

    public ImageService(IConfiguration configuration,IImageRepository imageRepository)
    {
        Configuration = configuration;
        var cloudName = Configuration.GetSection("CloudinarySettings").GetSection("CloudName").Value;
        var apiKey = Configuration.GetSection("CloudinarySettings").GetSection("ApiKey").Value;
        var apiSecret = Configuration.GetSection("CloudinarySettings").GetSection("ApiSecret").Value;
        var account = new Account(
            cloudName,
            apiKey,
            apiSecret
        );
        _cloudinary = new Cloudinary(account);
        _imageRepository = imageRepository;
    }
    public Image? UploadImage(IFormFile imFormFile)
    {
        var uploadResult = new ImageUploadResult();
        if (imFormFile.Length <= 0)
            return new Image()
            {
                Name = imFormFile.Name,
                Url = uploadResult.Url.ToString(),
            };
        using var stream = imFormFile.OpenReadStream();
        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(imFormFile.Name, stream)
        };
        uploadResult = _cloudinary.Upload(uploadParams);
        var image = new Image()
        {
            Name = imFormFile.Name,
            Url = uploadResult.Url.ToString(),
            PublicId = uploadResult.PublicId
        };
        _imageRepository.AddImage(image);
        return image;
    }

    public void DeleteImage(string publicId)
    {
       _cloudinary.Destroy(new DeletionParams(publicId));
    }

    public Image? FindImageByUrl(string url)
    {
        return _imageRepository.FindByImageUrl(url);
    }
}