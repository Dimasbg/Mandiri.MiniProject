using TicketingApp.Data.Api.EventApiRepo.Models;
using TicketingApp.Data.Dao;

namespace TicketingApp.Data.Api.EventApiRepo
{
    public interface IEventApiRepo
    {
        public Task<Event> Read(int id, CancellationToken c);
        public Task<List<Event>> List(EventListParamApiDataModel p, CancellationToken c);
    }
}
