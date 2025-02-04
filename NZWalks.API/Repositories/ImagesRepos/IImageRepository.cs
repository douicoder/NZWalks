using NZWalks.API.Models.Domains;

namespace NZWalks.API.Repositories.ImagesRepos
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
