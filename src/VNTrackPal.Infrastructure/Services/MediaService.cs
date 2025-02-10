using VNTrackPal.Contract.DTOs.MediaDTOs;


namespace VNTrackPal.Infrastructure.Services;

public sealed class MediaService : IMediaService
{
    private readonly CloudinarySetting _cloudinarySetting;
    private readonly Cloudinary _cloudinary;

    public MediaService(IOptions<CloudinarySetting> cloudinaryConfig)
    {
        var account = new Account(cloudinaryConfig.Value.CloudName,
            cloudinaryConfig.Value.ApiKey,
            cloudinaryConfig.Value.ApiSecret);

        _cloudinary = new Cloudinary(account);
        _cloudinarySetting = cloudinaryConfig.Value;
    }

    public async Task<bool> DeleteFileAsync(string publicId)
    {
        var deletionParams = new DeletionParams(publicId);
        var isCheck = await _cloudinary.DestroyAsync(deletionParams);
        if (isCheck.Error != null) return false;
        return true;
    }

    public async Task<ImageDTO> UploadImageAsync(string fileName, Stream fileStream)
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, fileStream),
            Folder = _cloudinarySetting.Folder,
        };
        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        if (uploadResult?.StatusCode != System.Net.HttpStatusCode.OK) return null;
        var imageUrl = uploadResult.Url.AbsoluteUri;
        var imageId = uploadResult.PublicId;
        return new ImageDTO(imageId, imageUrl);
    }

    public async Task<List<ImageDTO>> UploadImagesAsync(List<IFormFile> fileImages)
    {
        var imageDtoList = new List<ImageDTO>();

        foreach (var fileImage in fileImages)
        {
            var fileName = fileImage.FileName;

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, fileImage.OpenReadStream()),
                Folder = _cloudinarySetting.Folder,
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult?.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var imageUrl = uploadResult.Url.AbsoluteUri;
                var imageId = uploadResult.PublicId;

                imageDtoList.Add(new ImageDTO(imageId, imageUrl));
            }
        }

        return imageDtoList;
    }

    public async Task<VideoDTO?> UploadVideoAsync(string fileName, Stream fileStream)
    {
        var uploadParams = new VideoUploadParams
        {
            File = new FileDescription(fileName, fileStream),
            Folder = _cloudinarySetting.Folder,
        };
        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        if (uploadResult?.StatusCode != System.Net.HttpStatusCode.OK)
        {
            return null;
        }
        var videoId = uploadResult.PublicId;
        return new VideoDTO
        (
            PublicVideoId: videoId,
            Duration: uploadResult.Duration
        );
    }
}
