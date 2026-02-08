namespace WebApplication1.QueryParameters {

    public class GetRollQuery {
        public int Page { get; set; }
        public int PageSize { get; set; }

        // filtration
        public int? IdFrom { get; set; }
        public int? IdTo { get; set; }

        public float? WeightFrom { get; set; }
        public float? WeightTo { get; set; }

        public float? LengthFrom { get; set; }
        public float? LengthTo { get; set; }

        public DateTime? ReceiptedFrom { get; set; }
        public DateTime? ReceiptedTo { get; set; }

        public DateTime? RemovedFrom { get; set; }
        public DateTime? RemovedTo { get; set; }
    }
}
