using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Repositories.Interfaces;

namespace NzWalks.Api.Controllers
{
    [Controller]
    [Route("api/v1/[controller]")]
    public class WalkDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepo _walkDifficultyRepo;
        private readonly IMapper _mapper;

        public WalkDifficultyController(IWalkDifficultyRepo walkDifficultyRepo, IMapper mapper)
        {
            _walkDifficultyRepo = walkDifficultyRepo;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficulty()
        {
            var walkDifficulties = await _walkDifficultyRepo.GetAllAsync();
            var walkDifficultiesDto = _mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficulties);
            return Ok(walkDifficulties);
        }
        
        [HttpGet]
        [ActionName("GetWalkDifficulty")]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetWalkDifficulty(Guid id)
        {
            var walkDifficultyDomain = await _walkDifficultyRepo.GetAsync(id);
            var walkDifficultyDto = _mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);
            return Ok(walkDifficultyDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkDifficulty([FromBody]AddWalkDifficultyRequest request)
        {
           // if (!ValidateAddOrUpdateWalkDifficulty(request)) return BadRequest(ModelState);
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty()
            {
                Code = request.Code,
            };
            var addedWalkDifficulty = await _walkDifficultyRepo.AddAsync(walkDifficultyDomain);
            var addedWalkDifficultyDto = _mapper.Map<Models.DTO.WalkDifficulty>(addedWalkDifficulty);
            return CreatedAtAction(nameof(GetWalkDifficulty), new { id = addedWalkDifficultyDto.Id }, addedWalkDifficultyDto);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficulty([FromRoute]Guid id,[FromBody]AddWalkDifficultyRequest update)
        {
           // if (!ValidateAddOrUpdateWalkDifficulty(update)) return BadRequest(ModelState);
            var updateWalkDifficultyDomain = new Models.Domain.WalkDifficulty()
            {
                Code = update.Code,
            };
            var updatedWalkDifficulty = await _walkDifficultyRepo.UpdateAsync(id, updateWalkDifficultyDomain);
            if (updatedWalkDifficulty == null) return NotFound();
            var updatedWalkDifficultyDto = _mapper.Map<Models.DTO.WalkDifficulty>(updatedWalkDifficulty);
            return Ok(updatedWalkDifficultyDto);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficulty(Guid id)
        {
            var deletedWalkDifficulty = await _walkDifficultyRepo.DeleteAsync(id);
            if (deletedWalkDifficulty == null) return NotFound();
            return Ok(_mapper.Map<Models.DTO.WalkDifficulty>(deletedWalkDifficulty));
        }

        #region Private Methods
        private bool ValidateAddOrUpdateWalkDifficulty(AddWalkDifficultyRequest request)
        {
            if (request == null)
            {
                ModelState.AddModelError(nameof(request.Code), "Request body required");
                return false;
            }
            if(string.IsNullOrWhiteSpace(request.Code))
            {
                ModelState.AddModelError(nameof(request.Code), $"{nameof(request.Code)} cannot be null or empty or white spaced");
                return false;
            }
            return true;
        }
        
        #endregion
    }
}
