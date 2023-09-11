using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Repositories.Interfaces;

namespace NzWalks.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    
    public class RegionsController : Controller
    {
        private readonly IRegionRepo _regionRepo;
        private readonly IMapper _mapper;

        public RegionsController(IRegionRepo regionRepo,IMapper mapper)
        {
           _regionRepo = regionRepo;
            _mapper = mapper;
        }
        [HttpGet]
       
        public async Task<IActionResult> GetAllRegions()
        {
            var regions = await _regionRepo.GetAllAsync();
            var regionsDto = _mapper.Map<List<Models.DTO.Region>>(regions);
            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegion")]
       
        public async Task<IActionResult> GetRegion(Guid id)
        {
            var region = await _regionRepo.GetAsync(id);
            if(region is null)
                return NotFound();
            var regionDto = _mapper.Map<Models.DTO.Region>(region);
            return Ok(regionDto);
        }

        [HttpPost]
        
        public async Task<IActionResult> AddRegion(AddRegionRequest addRegion)
        {
           // if (!ValidateAddRegion(addRegion)) return BadRequest(ModelState);
            //Request DTO to domain model
            var region = new Models.Domain.Region()
            {
                Area = addRegion.Area,
                Code = addRegion.Code,
                Lat = addRegion.Lat,
                Long = addRegion.Long ,
                Name = addRegion.Name,
                Population = addRegion.Population,
            };
            //pass to repo
            var addedRegion =   await _regionRepo.AddAsync(region);
            //convert back to dto
            var addedRegionDto = _mapper.Map<Models.DTO.Region>(addedRegion);
            return CreatedAtAction(nameof(GetRegion),new { id = addedRegionDto.Id},addedRegionDto);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        
        public async Task<IActionResult> DeleteRegion(Guid id)
        {
            var deletedRegion = await _regionRepo.DeleteAsync(id);
            if (deletedRegion is null)
                return NotFound();
            var deletedRegionDto = _mapper.Map<Models.DTO.Region>(deletedRegion);
            return Ok(deletedRegionDto);
        }

        [HttpPut]
        [Route("{id:guid}")]
        
        public async Task<IActionResult> UpdateRegions([FromRoute]Guid id, [FromBody]AddRegionRequest updateRegion)
        {
           // if (!ValidateUpdateRegion(updateRegion)) return BadRequest(ModelState);
            var region = new Models.Domain.Region()
            {
                Area = updateRegion.Area,
                Code = updateRegion.Code,
                Lat = updateRegion.Lat,
                Long = updateRegion.Long,
                Name = updateRegion.Name,
                Population = updateRegion.Population
            };
            var updatedRegion = await _regionRepo.UpdateAsync(id, region);
            if(updatedRegion is null) return NotFound();
            var updatedRegionDto = _mapper.Map<Models.DTO.Region>(updatedRegion);
            return Ok(updatedRegionDto);
        }

        #region Private Methods
        private bool ValidateAddRegion(AddRegionRequest request)
        {
            if (request == null)
                ModelState.AddModelError(nameof(request), "Request body is required");

            if(string.IsNullOrWhiteSpace(request.Code))
            {
                ModelState.AddModelError(nameof(request.Code)
                    , $"{nameof(request.Code)} cannot be null or empty or white spaced");
            }
            if(string.IsNullOrWhiteSpace(request.Name))
            {
                ModelState.AddModelError(nameof(request.Name)
                    , "Name cannot be null or empty or white spaced");
            }
            if(request.Area <=0)
            {
                ModelState.AddModelError(nameof(request.Area)
                    , "Area cannot be less than or equal to zero");
            }
            if (request.Population < 0)
            {
                ModelState.AddModelError(nameof(request.Population)
                    , "Population cannot be less than zero");
            }

            if (ModelState.ErrorCount > 0) return false;

            return true;
        }
        private bool ValidateUpdateRegion(AddRegionRequest request)
        {
            return ValidateAddRegion(request);
        }
        #endregion
    }
}
