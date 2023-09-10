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
    public class WalkRepo : IWalkRepo
    {
        private readonly NzWalksDbContext _context;

        public WalkRepo(NzWalksDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await _context.Walks.
                Include(x => x.Region).
                Include(x => x.WalkDifficulty).ToListAsync();
        }

        public async Task<Walk> GetAsync(Guid id)
        {
            return await _context.Walks.Include(x => x.Region).Include(x => x.WalkDifficulty).
                FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            walk.Id = Guid.NewGuid();   
            await _context.Walks.AddAsync(walk);
            await _context.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await _context.Walks.FindAsync(id);
            if (existingWalk == null)
                return null;
            existingWalk.Length = walk.Length;
            existingWalk.WalkDifficultyId = walk.WalkDifficultyId;
            existingWalk.Name = walk.Name;
            existingWalk.RegionId = walk.RegionId;
            _context.Walks.Update(existingWalk);
            await _context.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var existingWalk = await _context.Walks.FindAsync(id);
            if(existingWalk != null)
            {
                _context.Walks.Remove(existingWalk);
                await _context.SaveChangesAsync();
                return existingWalk;
            }
            return null;
        }
    }
}
