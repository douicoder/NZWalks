using AutoMapper;
using NZWalks.API.Models.Domains;
using NZWalks.API.Models.DTO;
using NZWalks.API.Models.DTO.DifficultyDtos;
using NZWalks.API.Models.DTO.Walksdtos;

namespace NZWalks.API.Mappings
{
    public class AutoMapperProfiles :Profile
    {
        public AutoMapperProfiles()
        {
            //You also gotta inject the auto mapper in program.cs
            //formapping differet values with different names
            /*   CreateMap<UserDTO,UserDomain>()
                   .ForMember(x =>x.Name,opt =>opt).MapFrom(x=>x.Fullname)).ReverseMap()*/

            //Regions
            CreateMap<Region, RegionDTO>().ReverseMap();
            CreateMap<AddRegionDTO, Region>().ReverseMap();
            CreateMap<UpdateRegionDTO, Region>().ReverseMap();
            //Walks
            CreateMap<AddWalkRequestDto, Walk>().ReverseMap();
            CreateMap<WalksDto, Walk>().ReverseMap();
            CreateMap<UpdateWalkRequestDto, Walk>().ReverseMap();

            //Difficulty
            CreateMap<Difficulty,DifficultyDto>().ReverseMap();

        }
    }
}
