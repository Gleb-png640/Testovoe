namespace WebApplication1.Models.Entities
{
    public class EntityRoll
    {
        public long Id { get; set; }
        public float Weight { get; set; }
        public float Length { get; set; }
        public DateTime ReceiptDate { get; set; }
        public DateTime? RemovedDate { get; set; } = null;
    }
}
