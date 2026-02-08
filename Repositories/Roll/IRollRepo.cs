using WebApplication1.Models.Dtos;
using WebApplication1.Models.Entities;
using WebApplication1.QueryParameters;

namespace WebApplication1.Repositories.Roll
{
    public interface IRollRepo
    {
        public IEnumerable<EntityRoll> GetPaged(GetRollQuery query);

        public EntityRoll Add(CreateDtoRoll dto);

        public void Delete(EntityRoll roll);

        public EntityRoll? FindById(long id);
    }
}
