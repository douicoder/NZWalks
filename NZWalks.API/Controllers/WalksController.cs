using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domains;
using NZWalks.API.Models.DTO.Walksdtos;
using NZWalks.API.Repositories.WalksRepositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper,IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        [HttpPost]
        [ValidateModel]

        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto) 
        {
          
                //Map dto to domain
                var walkdomainModel = mapper.Map<Walk>(addWalkRequestDto);
                await walkRepository.CreateAsync(walkdomainModel);
                //Map dominmodel to Dto
                return Ok(mapper.Map<WalksDto>(walkdomainModel));
           
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filteron, [FromQuery] string? filterquery, 
            [FromQuery] string? sortBy, [FromQuery]bool? IsAscending,
            [FromQuery] int pageNumber = 1, [FromQuery]int pageSize=1000) 
        {
            var walksDomainModel=await walkRepository.GetAllAsync(filteron,filterquery,sortBy,IsAscending??true,pageNumber,pageSize);
            //Mapping the domain to Dto
            return Ok(mapper.Map< List<WalksDto>>(walksDomainModel));
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id) 
        {
            var walkDomainModel=await walkRepository.GetByIdAsync(id);
            if (walkDomainModel == null) 
            {
                return NotFound();
            }
            //mapping domain to dto
            return Ok(mapper.Map<WalksDto>(walkDomainModel));
        }
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {

                //mapping dto to daomain
                var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);

                walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);
                if (walkDomainModel == null)
                {
                    return NotFound();
                }
                //mapping domain to dto
                return Ok(mapper.Map<WalksDto>(walkDomainModel));

        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) 
        {
           var deletedWalkDomainModel= await walkRepository.DeleteAsync(id);
            if(deletedWalkDomainModel == null) 
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalksDto>(deletedWalkDomainModel));

        }
    
    
    }
}
