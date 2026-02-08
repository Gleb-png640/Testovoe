using WebApplication1.Models.Dtos;
using WebApplication1.Models.Entities;
using WebApplication1.QueryParameters;

namespace WebApplication1.Repositories.Roll
{
    public interface IRollRepo
    {
        public Task<IEnumerable<EntityRoll>> GetPagedAsync(GetRollQuery query);

        public Task<EntityRoll> AddAsync(CreateDtoRoll dto);

        public Task DeleteAsync(EntityRoll roll);

        public Task<EntityRoll?> FindByIdAsync(long id);
    }
}
