using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.Dtos;
using WebApplication1.QueryParameters;

namespace WebApplication1.Repositories.Stats
{

    // Простите

    public class StatsQuery {
        public int ReceiptedCount { get; set; }
        public float? MaxLength { get; set; }
        public float? MinLength { get; set; }
        public float? MaxWeight { get; set; }
        public float? MinWeight { get; set; }
        public float? TotalWeight { get; set; }
        public float AvgLength { get; set; }
        public float AvgWeight { get; set; }
        public bool HasRemoved { get; set; }
    };

    public class RemovedStats {
        public int RemovedCount { get; set; } = 0;
        public TimeSpan? MinInterval { get; set; }
        public TimeSpan? MaxInterval { get; set; }
    };

    public class DailyExtremes {
        public DateTime? MinCountDate { get; set; }
        public int? MinCount { get; set; }
        public DateTime? MaxCountDate { get; set; }
        public int? MaxCount { get; set; }
        public DateTime? MinWeightDate { get; set; }
        public float? MinWeight { get; set; }
        public DateTime? MaxWeightDate { get; set; }
        public float? MaxWeight { get; set; }
    };


    public class StatsRepo : IStatsRepo {

        private SeverstalDbContext _db;

        public StatsRepo(SeverstalDbContext dbContext) {
            _db = dbContext;

            if (!_db.Database.CanConnect()) {
                throw new Exception("Не удалось подключиться к БД");
            }
        }

        public RollStatsDto? GetStats(GetStatsQuery dto) {

            var statsQuery = FormStatsQuery(dto);

            if (statsQuery is null) { return null; }


            var stats = new RollStatsDto();

            int receiptedCount = _db.Rolls.Count(r => r.ReceiptDate >= dto.PeriodStart && 
                                                    r.ReceiptDate <= dto.PeriodEnd);
            
            if (statsQuery.HasRemoved) {
                var removedStats = FormRemovedStats(dto);
                ApplyRemovedStats(stats, removedStats);
            }

            ApplyGeneralStats(stats, dto, statsQuery, receiptedCount);

            var dailyExtremes = FormDailyExtremes(dto);
            ApplyDailyExtremes(stats, dailyExtremes);
            return stats;
        }


        private StatsQuery? FormStatsQuery(GetStatsQuery dto) {

            var statsQuery = _db.Rolls
                    .Where(r => r.ReceiptDate <= dto.PeriodEnd &&
                            (r.RemovedDate == null || r.RemovedDate >= dto.PeriodStart))
                    .GroupBy(r => 1)
                    .Select(g => new StatsQuery {
                        ReceiptedCount = g.Count(),
                        AvgLength = g.Average(r => r.Length),
                        AvgWeight = g.Average(r => r.Weight),
                        MaxLength = g.Max(r => r.Length),
                        MinLength = g.Min(r => r.Length),
                        MaxWeight = g.Max(r => r.Weight),
                        MinWeight = g.Min(r => r.Weight),
                        TotalWeight = g.Sum(r => r.Weight),
                        HasRemoved = g.Any(r => r.RemovedDate != null &&
                                            r.RemovedDate >= dto.PeriodStart &&
                                            r.RemovedDate <= dto.PeriodEnd)
                    })
                    .FirstOrDefault();

            return statsQuery;
        }

        private void ApplyGeneralStats(RollStatsDto stats, GetStatsQuery dto, StatsQuery statsQuery, int receiptedCount) {
            stats.PeriodStart = dto.PeriodStart;
            stats.PeriodEnd = dto.PeriodEnd;
            stats.ReceiptedCount = receiptedCount;
            stats.AvgLength = (float)statsQuery.AvgLength;
            stats.AvgWeight = (float)statsQuery.AvgWeight;
            stats.MaxLength = (float)statsQuery.MaxLength!;
            stats.MinLength = (float)statsQuery.MinLength!;
            stats.MaxWeight = (float)statsQuery.MaxWeight!;
            stats.MinWeight = (float)statsQuery.MinWeight!;
            stats.TotalWeight = (float)statsQuery.TotalWeight!;
        }

        private RemovedStats FormRemovedStats( GetStatsQuery dto) {
            RemovedStats removedStats = _db.Rolls
                        .Where(r => r.RemovedDate != null &&
                                   r.RemovedDate >= dto.PeriodStart &&
                                   r.RemovedDate <= dto.PeriodEnd)
                        .GroupBy(r => 1)
                        .Select(g => new RemovedStats {
                            RemovedCount = g.Count(),
                            MinInterval = g.Min(r => (r.RemovedDate!.Value - r.ReceiptDate))!,
                            MaxInterval = g.Max(r => (r.RemovedDate!.Value - r.ReceiptDate))!
                        })
                        .First();

            return removedStats;
        }

        private void ApplyRemovedStats(RollStatsDto stats, RemovedStats removedStats) {       

            stats.RemovedCount = removedStats.RemovedCount;

            stats.MinInterval = removedStats.MinInterval;
            stats.MaxInterval = removedStats.MaxInterval;
        }

        private DailyExtremes FormDailyExtremes(GetStatsQuery dto) {

            DailyExtremes result = new();

            int? minRolls = null;
            int? maxRolls = null;
            float? minWeight = null;
            float? maxWeight = null;

            DateTime day = dto.PeriodStart.Date;
            DateTime end = dto.PeriodEnd.Date;

            while (day <= end) {

                var rollsAtDay = _db.Rolls
                    .Where(r =>
                        r.ReceiptDate <= day.AddDays(1).AddTicks(-1) &&
                        (r.RemovedDate == null || r.RemovedDate > day)
                    );

                int count = rollsAtDay.Count();
                float weight = rollsAtDay.Sum(r => (float?)r.Weight) ?? 0;

                if (minRolls == null || count < minRolls) {
                    minRolls = count;
                    result.MinCountDate = day;
                }

                if (maxRolls == null || count > maxRolls) {
                    maxRolls = count;
                    result.MaxCountDate = day;
                }

                if (minWeight == null || weight < minWeight) {
                    minWeight = weight;
                    result.MinWeightDate = day;
                }

                if (maxWeight == null || weight > maxWeight) {
                    maxWeight = weight;
                    result.MaxWeightDate = day;
                }

                day = day.AddDays(1);
            }

            return result;
        }

        private void ApplyDailyExtremes(RollStatsDto stats, DailyExtremes dailyExtremes) {

            stats.MinRollsDay = dailyExtremes.MinCountDate;
            stats.MaxRollsDay = dailyExtremes.MaxCountDate;

            stats.MinWeightDay = dailyExtremes.MinWeightDate;
            stats.MaxWeightDay = dailyExtremes.MaxWeightDate;
        }
    }

}
