using TicketingApp.Data.Dao;

namespace TicketingApp.Api.Models.PurchaseModels
{
    public class PurchaseApiModel
    {
        public int UserId { get; set; }
        public int TicketId { get; set; }
        public DateTime? PurchaseDateTime { get; set; }
        public int Quantity { get; set; }
        public Purchase ToDao(int id = 0)
            => new Purchase
            {
                PurchaseDateTime = PurchaseDateTime,
                Quantity = Quantity,
                Ticket = new Ticket { TicketId = TicketId },
                UserId = UserId,
                PurchaseId = id
            };
    }
}
