using TicketingApp.Data.Dao;
using TicketingApp.Domain.PurchaseService.Models;

namespace TicketingApp.Domain.PurchaseService
{
    public interface IPurchaseService
    {
        public Task<Purchase> Create(Purchase d, CancellationToken c);
        public Task<Purchase> Read(int id, CancellationToken c, bool isIncludeTicket = false);
        public Task<Purchase> Update(Purchase d, CancellationToken c);
        public Task Delete(int id, CancellationToken c);
        public Task<List<Purchase>> List(PurchaseListParamDomainModel p, CancellationToken c);
    }
}
