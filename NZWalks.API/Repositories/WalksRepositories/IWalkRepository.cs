using NZWalks.API.Models.Domains;

namespace NZWalks.API.Repositories.WalksRepositories
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk walk);
        Task<List<Walk>> GetAllAsync(string? filteron=null,string? filterquery=null,
                                               string? sortby=null ,bool? IsAscending=true,
                                               int pageNumber=1,int pageSize=1000);
        Task<Walk?> GetByIdAsync(Guid id);
        Task<Walk?> UpdateAsync(Guid id,Walk walk);
        Task<Walk?> DeleteAsync(Guid id);
    }
}
