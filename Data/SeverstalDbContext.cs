using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.Entities;

namespace WebApplication1.Data
{
    public class SeverstalDbContext(DbContextOptions<SeverstalDbContext> options): DbContext(options) 
    {
        public DbSet<EntityRoll> Rolls => Set<EntityRoll>();

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            builder.Entity<EntityRoll>(entity => 
            {

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });
        }
    }
}
