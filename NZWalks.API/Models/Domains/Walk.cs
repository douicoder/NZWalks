namespace NZWalks.API.Models.Domains
{
    public class Walk
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Decription{ get; set; }
        public string LengthInKm{ get; set; }
        public string TheWalkImageUrl{ get; set; }

        public Guid DifficultyID { get; set; }


        public Guid RegionId { get; set; }

        //NAvigation properties

        public Difficulty Difficulty { get; set; }
        public Region Region{ get; set; }
    }
}
