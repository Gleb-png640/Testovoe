using WebApplication1.Models.Dtos;
using WebApplication1.Models.Entities;

namespace WebApplication1.Models.Mapping {
    public static class RollsMapping {

        public static EntityRoll DtoToEntity(this CreateDtoRoll dto) {
            return new EntityRoll {
                Weight = dto.Weight,
                Length = dto.Length,
                ReceiptDate = DateTime.UtcNow
            };
        }
    }
}
