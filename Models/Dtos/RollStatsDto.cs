namespace WebApplication1.Models.Dtos {
    public class RollStatsDto {

        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }

        public int ReceiptedCount { get; set; }
        public int RemovedCount { get; set; }

        public float AvgLength { get; set; }
        public float AvgWeight { get; set; }

        public float MaxLength { get; set; }
        public float MinLength { get; set; }

        public float MaxWeight { get; set; }
        public float MinWeight { get; set; }

        public float TotalWeight { get; set; }

        public TimeSpan? MaxInterval { get; set; }
        public TimeSpan? MinInterval { get; set; }


        public DateTime? MinRollsDay { get; set; }
        public DateTime? MaxRollsDay { get; set; }

        public DateTime? MinWeightDay { get; set; }
        public DateTime? MaxWeightDay { get; set; }
    }
}
