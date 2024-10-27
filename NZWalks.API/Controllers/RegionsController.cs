using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domains;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        public RegionsController(NZWalksDbContext dbContext)
        {
           this.dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult GetAll() 
        {
          var regionsdomain = dbContext.Regions.ToList();
            var regionsDto = new List<RegionDTO>();
            foreach (var regionsdomains in regionsdomain) 
            {
                regionsDto.Add(new RegionDTO() 
                {
                Id = regionsdomains.Id,
                Code = regionsdomains.Code,
                Name = regionsdomains.Name,
                RegionImageUrl = regionsdomains.RegionImageUrl
                });
            }

            return Ok(regionsDto);
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute]Guid id)
        {
            //Can be used with primary key only

            //getting domain
            var regionsdomain = dbContext.Regions.Find(id);
            ///Can be used for any vriable i.e. insted of id you can use anything like name,cade etc..
            //var regions = dbContext.Regions.FirstOrDefault(x=>x.Id==id);

            if (regionsdomain == null) { 
            return NotFound();
            }
            //Mapping Domian to DTO
            var regionDtos = new RegionDTO 
            {
                Id = regionsdomain.Id,
                Code = regionsdomain.Code,
                Name = regionsdomain.Name,
                RegionImageUrl = regionsdomain.RegionImageUrl
            };

            return Ok(regionDtos);

        }

        [HttpPost]
        public IActionResult Create([FromBody] AddRegionDTO dto) 
        {
            var regionDomain = new Region
            {
                
                Code = dto.Code,
                Name = dto.Name,
                RegionImageUrl = dto.RegionImageUrl,
            };
            dbContext.Regions.Add(regionDomain);
            dbContext.SaveChanges();
            var regionDto = new RegionDTO
            {
                Id=regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl=regionDomain.RegionImageUrl,
            };
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        //Update
        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Update([FromRoute]Guid id, [FromBody]UpdateRegionDTO updateRegionDTO) 
        {
           var regionDomainModel= dbContext.Regions.FirstOrDefault(x=>x.Id==id);
            if (regionDomainModel == null) 
            {
            return NotFound();
            }

            regionDomainModel.Code=updateRegionDTO.Code;
            regionDomainModel.Name=updateRegionDTO.Name;
            regionDomainModel.RegionImageUrl = updateRegionDTO.RegionImageUrl;

            dbContext.SaveChanges();
            var regionDto = new RegionDTO
            {
                Id= regionDomainModel.Id,
                Code=regionDomainModel.Code,
                Name=regionDomainModel.Name,
                RegionImageUrl=regionDomainModel.RegionImageUrl
            };
            return Ok(regionDto);


        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete([FromRoute] Guid id) 
        {
            var regionDomainModel = dbContext.Regions.FirstOrDefault(x=>x.Id==id);
            if (regionDomainModel == null) { return NotFound(); }
            dbContext.Regions.Remove(regionDomainModel);
            dbContext.SaveChanges();

            var regionDto = new RegionDTO
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return Ok(regionDto);



        }


    }
}
