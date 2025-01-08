using NZWalks.API.Models.DTO.DifficultyDtos;

namespace NZWalks.API.Models.DTO.Walksdtos
{
    public class WalksDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Decription { get; set; }
        public string LengthInKm { get; set; }
        public string? TheWalkImageUrl { get; set; }


        //navigaton properties
        public DifficultyDto Difficulty { get; set; }
        public RegionDTO Region { get; set; }
    }
}
