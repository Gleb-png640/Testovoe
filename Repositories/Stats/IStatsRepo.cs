using WebApplication1.Models.Dtos;
using WebApplication1.QueryParameters;

namespace WebApplication1.Repositories.Stats
{

    public interface IStatsRepo {

        public Task<RollStatsDto?> GetStatsAsync(GetStatsQuery dto);
    }
}
