using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Repositories.Interfaces;
using System.Reflection.Metadata.Ecma335;

namespace NzWalks.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepo _walkRepo;
        private readonly IMapper _mapper;
        private readonly IRegionRepo _regionRepo;
        private readonly IWalkDifficultyRepo _walkDiffRepo;

        public WalksController(
            IWalkRepo walkRepo,
            IMapper mapper,
            IRegionRepo regionRepo,
            IWalkDifficultyRepo walkDifficultyRepo)
        {
            _walkRepo = walkRepo;
            _mapper = mapper;
            _regionRepo = regionRepo;
            _walkDiffRepo = walkDifficultyRepo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWalks()
        {
            var walks = await _walkRepo.GetAllAsync();
            var walksDto = _mapper.Map<List<Models.DTO.Walk>>(walks);
            return Ok(walksDto);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalk")]
        public async Task<IActionResult> GetWalk(Guid id)
        {
            var walk = await _walkRepo.GetAsync(id);
            var walkDto = _mapper.Map<Models.DTO.Walk>(walk);
            return Ok(walkDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalk(AddWalkRequest request)
        {
            var isValid = await ValidateAddWalk(request);
            if (!isValid) return BadRequest(ModelState);
            var addWalkDomain = new Models.Domain.Walk()
            {
                Length = request.Length,
                Name = request.Name,
                RegionId = request.RegionId,
                WalkDifficultyId = request.WalkDifficultyId
            };
            var addedWalk = await _walkRepo.AddAsync(addWalkDomain);
            var addedWalkDto = _mapper.Map<Models.DTO.Walk>(addedWalk);
            return CreatedAtAction(nameof(GetWalk),new { id = addedWalkDto.Id },addedWalkDto);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalk([FromRoute]Guid id, [FromBody]AddWalkRequest update)
        {
            var isValid = await ValidateUpdateWalk(update);
            if (!isValid) return BadRequest(ModelState);
            var updateWalkDomain = new Models.Domain.Walk()
            {
                Length = update.Length,
                Name = update.Name,
                RegionId = update.RegionId,
                WalkDifficultyId = update.WalkDifficultyId
            };
            var updatedWalk = await _walkRepo.UpdateAsync(id, updateWalkDomain);
            if(updatedWalk != null)
            {
                var updatedWalkDto = _mapper.Map<Models.DTO.Walk>(updatedWalk);
                return Ok(updatedWalkDto);
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalk(Guid id)
        {
            var walk = await _walkRepo.DeleteAsync(id);
            if(walk != null)
                return Ok(_mapper.Map<Models.DTO.Walk>(walk));
            return NotFound();
        }

        #region Private Methods
        private async Task<bool> ValidateAddWalk(AddWalkRequest request)
        {
            if(request == null)
            {
                ModelState.AddModelError(nameof(request), "Request body cannot be null");
                return false;
            }
            if(string.IsNullOrWhiteSpace(request.Name))
            {
                ModelState.AddModelError(nameof(request.Name), $"{nameof(request.Name)} cannot be null or empty or white spaced");
            }
            if(request.Length <=0)
            {
                ModelState.AddModelError(nameof(request.Length), $"{nameof(request.Length)} cannot be less than or equal to zer");
            }
            var region = await _regionRepo.GetAsync(request.RegionId);
            if (region == null) ModelState.AddModelError(nameof(request.RegionId), $"{nameof(request.RegionId)} is invalid");
            var walkDiff = await _walkDiffRepo.GetAsync(request.WalkDifficultyId);
            if (walkDiff == null) ModelState.AddModelError(nameof(request.WalkDifficultyId), $"{nameof(request.WalkDifficultyId)} is invalid");
            if (ModelState.ErrorCount > 0) return false;
            return true;
        }

        private async Task<bool> ValidateUpdateWalk(AddWalkRequest update)
        {
            var flag =  await ValidateAddWalk(update);
            return flag;
        }
        #endregion
    }
}
