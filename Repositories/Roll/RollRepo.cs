using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.Dtos;
using WebApplication1.Models.Entities;
using WebApplication1.Models.Mapping;
using WebApplication1.QueryParameters;

namespace WebApplication1.Repositories.Roll
{
    public class RollRepo : IRollRepo
    {

        private SeverstalDbContext _db;

        public RollRepo(SeverstalDbContext dbContext) {
            _db = dbContext;

            if (!_db.Database.CanConnect()) {
                throw new Exception("Не удалось подключиться к БД");
            }
        }

        public async Task<IEnumerable<EntityRoll>> GetPagedAsync(GetRollQuery query)
        {

            IQueryable<EntityRoll> rolls = ApplyFilters(query);

            const int pageOffset = 1;
            int page = query.Page;
            int pageSize = query.PageSize;

            return await rolls
                .OrderBy(r => r.Id)
                .Skip((page - pageOffset) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        private IQueryable<EntityRoll> ApplyFilters(GetRollQuery query)
        {

            IQueryable<EntityRoll> rolls = _db.Rolls;

            if (query.IdFrom.HasValue) { rolls = rolls.Where(r => r.Id >= query.IdFrom); }

            if (query.IdTo.HasValue) { rolls = rolls.Where(r => r.Id <= query.IdTo); }

            if (query.WeightFrom.HasValue) { rolls = rolls.Where(r => r.Weight >= query.WeightFrom); }

            if (query.WeightTo.HasValue) { rolls = rolls.Where(r => r.Weight <= query.WeightTo); }

            if (query.LengthFrom.HasValue) { rolls = rolls.Where(r => r.Length >= query.LengthFrom); }

            if (query.LengthTo.HasValue) { rolls = rolls.Where(r => r.Length <= query.LengthTo); }

            if (query.ReceiptedFrom.HasValue) { rolls = rolls.Where(r => r.ReceiptDate >= query.ReceiptedFrom); }

            if (query.ReceiptedTo.HasValue) { rolls = rolls.Where(r => r.ReceiptDate <= query.ReceiptedTo); }

            if (query.RemovedFrom.HasValue) { rolls = rolls.Where(r => r.RemovedDate >= query.RemovedFrom); }

            if (query.RemovedTo.HasValue) { rolls = rolls.Where(r => r.RemovedDate <= query.RemovedTo); }

            return rolls;
        }


        public async Task<EntityRoll?> FindByIdAsync(long id)
        {
            return await _db.Rolls.FindAsync(id);
        }


        public async Task DeleteAsync(EntityRoll roll)
        {
            roll.RemovedDate = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }


        public async Task<EntityRoll> AddAsync(CreateDtoRoll dto)
        {

            EntityRoll roll = dto.DtoToEntity();

            _db.Rolls.Add(roll);
            await _db.SaveChangesAsync();

            return roll;
        }
    }
}
