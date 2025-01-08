
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
        { 

        }
            //ea171c81-0a87-47c6-b96b-f4f7e50ff467
            //46a7f807-a54d-4c99-aa73-ef50b20b3bd2
            protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder);
                var readerRoleId = "ea171c81-0a87-47c6-b96b-f4f7e50ff467";
                var writerRoleId = "46a7f807-a54d-4c99-aa73-ef50b20b3bd2";
            var role = new List<IdentityRole>
                {
                    new IdentityRole
                    {
                        Id = readerRoleId,
                        ConcurrencyStamp=readerRoleId,
                        Name="Reader",
                        NormalizedName="Reader".ToUpper()
                    },
                    new IdentityRole
                    {
                        Id = writerRoleId,
                        ConcurrencyStamp=writerRoleId,
                        Name="Writer",
                        NormalizedName="Writer".ToUpper()
                    },

                };
                builder.Entity<IdentityRole>().HasData(role);
            } 
    }
 }



