using Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IWalkDifficultyRepo
    {
        Task<IEnumerable<WalkDifficulty>> GetAllAsync();
        Task<WalkDifficulty> GetAsync(Guid id);
        Task<WalkDifficulty> AddAsync(WalkDifficulty item);
        Task<WalkDifficulty> UpdateAsync(Guid id,WalkDifficulty item);
        Task<WalkDifficulty> DeleteAsync(Guid id);
    }
}
