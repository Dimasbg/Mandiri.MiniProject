namespace FeedBackApp.Data.Dao
{
    public class Purchase
    {
        public int PurchaseId { get; set; }
        public int UserId { get; set; }
        public DateTime? PurchaseDateTime { get; set; }
        public int Quantity { get; set; }
    }
}
