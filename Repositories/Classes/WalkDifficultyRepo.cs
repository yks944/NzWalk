using Microsoft.EntityFrameworkCore;
using Models.Data;
using Models.Domain;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Classes
{
    public class WalkDifficultyRepo : IWalkDifficultyRepo
    {
        private readonly NzWalksDbContext _context;

        public WalkDifficultyRepo(NzWalksDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await _context.WalkDifficulty.ToListAsync();   
        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            var walkDiff = await _context.WalkDifficulty.FindAsync(id);
            return walkDiff;
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = Guid.NewGuid();
            await _context.WalkDifficulty.AddAsync(walkDifficulty);
            await _context.SaveChangesAsync();
            return walkDifficulty;
        }
        
        public async Task<WalkDifficulty> UpdateAsync(Guid id,WalkDifficulty walkDifficulty)
        {
            var exisitngWalkDifficulty = await _context.WalkDifficulty.FindAsync(id);
            if (exisitngWalkDifficulty == null) return null;
            exisitngWalkDifficulty.Code = walkDifficulty.Code;
            _context.WalkDifficulty.Update(exisitngWalkDifficulty);
            await _context.SaveChangesAsync();
            return exisitngWalkDifficulty;
        }
        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            var exisitngWalkDifficulty = await _context.WalkDifficulty.FindAsync(id);
            if (exisitngWalkDifficulty == null) return null;
            _context.WalkDifficulty.Remove(exisitngWalkDifficulty);
            await _context.SaveChangesAsync();
            return exisitngWalkDifficulty;
        }
    }
}
