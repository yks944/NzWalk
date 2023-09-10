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
    public class RegionRepo : IRegionRepo
    {
        private readonly NzWalksDbContext _context;
        public RegionRepo(NzWalksDbContext context)
        {
            this._context = context;
        }

        public async Task<Region> AddAsync(Region region)
        {
            region.Id = Guid.NewGuid();
            await _context.AddAsync(region);
            await _context.SaveChangesAsync();
            return region;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await _context.Regions.ToListAsync();
        }

        public async Task<Region> GetAsync(Guid id)
        {
            return await _context.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
            var region = await _context.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (region == null)
                return null;
            _context.Regions.Remove(region);
            await _context.SaveChangesAsync();
            return region;
        }

        public async Task<Region> UpdateAsync(Guid id,Region region)
        {
            var exisitingRegion = await _context.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if(exisitingRegion is null)
            {
                return null;
            }
            exisitingRegion.Area = region.Area;
            exisitingRegion.Code = region.Code;
            exisitingRegion.Name = region.Name; ;
            exisitingRegion.Population = region.Population;
            exisitingRegion.Lat = region.Lat;
            exisitingRegion.Long = region.Long;
            _context.Regions.Update(exisitingRegion);
            await _context.SaveChangesAsync();
            return exisitingRegion;
        }
    }
}
