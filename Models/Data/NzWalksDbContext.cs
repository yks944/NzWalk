using Microsoft.EntityFrameworkCore;
using Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data
{
    public class NzWalksDbContext : DbContext
    {
        public NzWalksDbContext(DbContextOptions<NzWalksDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           /* modelBuilder.Entity<User_Role>().
                HasOne(x => x.Role).WithMany(y => y.UserRoles).HasForeignKey(z => z.RoleId);

            modelBuilder.Entity<User_Role>().
                HasOne(x => x.User).WithMany(y => y.UserRoles).HasForeignKey(z => z.UserId);*/
        }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<WalkDifficulty> WalkDifficulty { get; set; }
      
    }
}
