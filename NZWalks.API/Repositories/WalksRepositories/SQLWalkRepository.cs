using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domains;

namespace NZWalks.API.Repositories.WalksRepositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var existingwalk = await dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
            if (existingwalk == null)
            {
                return null;
            }
            dbContext.Walks.Remove(existingwalk);
            await dbContext.SaveChangesAsync();
            return existingwalk;
        }

        public async Task<List<Walk>> GetAllAsync(string? filteron = null, string? filterquery = null, string? sortby = null, bool? IsAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var walks= dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();
            //filtering
            if (string.IsNullOrWhiteSpace(filteron) == false && string.IsNullOrWhiteSpace(filterquery)==false) 
            {
                if (filteron.Equals("Name", StringComparison.OrdinalIgnoreCase)) 
                {
                  walks=walks.Where(x=>x.Name.Contains(filterquery));
                }
            }
            //sorting(hey this thig will work for length BUT I MADE THE LENGTH IN STRING SHOYLD HAVE BEEN INT OR DOUBLE SO IT DONT WORK BUT IT WILL WORK IF IT WAS DONE CORRECTLY)
            if (string.IsNullOrWhiteSpace(sortby) == false) 
            {
                if(sortby.Equals("Name", StringComparison.OrdinalIgnoreCase)) 
                {
                    walks= (bool)IsAscending ? walks.OrderBy(x=>x.Name): walks.OrderByDescending(x=>x.Name);
                }
                else if (sortby.Equals("Length", StringComparison.OrdinalIgnoreCase)) 
                {
                    walks= (bool)IsAscending ? walks.OrderBy(x=>x.LengthInKm):walks.OrderByDescending(x=>x.LengthInKm);
                }
            }
            //pagination
            //define -Pagination is a method of dividing content into pages or smaller sections
            var skipresults = (pageNumber - 1) * pageSize;
            return await walks.Skip(skipresults).Take(pageSize).ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {

            return await dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x=>x.Id==id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
           var existingwalk=await dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x=>x.Id==id);
            if (existingwalk == null)
            {
                return null;
            }

            existingwalk.Name = walk.Name;
            existingwalk.Decription = walk.Decription;
            existingwalk.LengthInKm = walk.LengthInKm;
            existingwalk.TheWalkImageUrl = walk.TheWalkImageUrl;
            existingwalk.DifficultyID = walk.DifficultyID;
            existingwalk.RegionId = walk.RegionId;

            await dbContext.SaveChangesAsync();
            return existingwalk;

        }
    }
}
