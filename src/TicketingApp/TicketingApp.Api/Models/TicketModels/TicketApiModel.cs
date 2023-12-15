using TicketingApp.Data.Dao;

namespace TicketingApp.Api.Models.TicketModels
{
    public class TicketApiModel
    {
        public int EventId { get; set; }
        public string? TicketType { get; set; }
        public decimal Price { get; set; }
        public Ticket ToDao(int id = 0)
            => new Ticket
            {
                EventId = EventId,
                TicketType = TicketType,
                Price = Price,
                TicketId = id
            };
    }
}
