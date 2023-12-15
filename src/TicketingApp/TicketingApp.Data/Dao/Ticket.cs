namespace TicketingApp.Data.Dao
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public int EventId { get; set; }
        public string? TicketType { get; set; }
        public decimal Price { get; set; }

        public void Update(Ticket d)
        {
            EventId = d.EventId;
            TicketType = d.TicketType;
            Price = d.Price;
        }
    }
}
