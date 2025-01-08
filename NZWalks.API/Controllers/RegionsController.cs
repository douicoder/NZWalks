using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domains;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController] 
    [Authorize]

    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly NZWalksAuthDbContext dbContextauth;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper, NZWalksAuthDbContext _dbContextauth)
        {
           this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            dbContextauth = _dbContextauth;
        }
        [Authorize]
        [HttpGet]
        //[AllowAnonymous]
        public async Task<IActionResult> GetAll() 
        {
          var regionsdomain = await regionRepository.GetAllAsync();
            //Maps Domains to dto
          var regionsDto = mapper.Map<List<RegionDTO>>(regionsdomain);

            return Ok(regionsDto);
        }


        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            //Can be used with primary key only

            //getting domain
         //   var regionsdomain = dbContext.Regions.Find(id);
            ///Can be used for any vriable i.e. insted of id you can use anything like name,cade etc..
            var regionsdomain =await regionRepository.GetByIdAsync(id);

            if (regionsdomain == null) { 
            return NotFound();
            }
            //Mapping Domian to DTO
           return Ok(mapper.Map<RegionDTO>(regionsdomain));

        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddRegionDTO dto) 
        {
                var regionDomain = mapper.Map<Region>(dto);
                regionDomain = await regionRepository.CreateAsync(regionDomain);

                //mapping domain to Dto
                var regionDto = mapper.Map<RegionDTO>(regionDomain);
                return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        
        }

        //Update
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute]Guid id, [FromBody]UpdateRegionDTO updateRegionDTO) 
        {
         
                //Maping DTO to DOMAIN
                var regionDomainModel = mapper.Map<Region>(updateRegionDTO);
                regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);
                if (regionDomainModel == null)
                {
                    return NotFound();
                }
                //Maping from domain to DTO
                var regionDto = mapper.Map<RegionDTO>(regionDomainModel);
                return Ok(regionDto);
  
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) 
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            if (regionDomainModel == null) { return NotFound(); }
            //Mapping domain to Dto
            var regionDto = mapper.Map<RegionDTO>(regionDomainModel);
            return Ok(regionDto);



        }


    }
}
    