namespace TicketingApp.Data.Dao
{
    public class Purchase
    {
        public int PurchaseId { get; set; }
        public int UserId { get; set; }
        public virtual Ticket Ticket { get; set; }
        public DateTime? PurchaseDateTime { get; set; }
        public int Quantity { get; set; }

        public void Update(Purchase d)
        {
            if (d.PurchaseDateTime.HasValue)
                PurchaseDateTime = d.PurchaseDateTime;
            Quantity = d.Quantity;
            Ticket = d.Ticket;
            UserId = d.UserId;
        }
    }
}
