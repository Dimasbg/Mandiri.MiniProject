using NotificationApp.Data.Api.EventApiRepo.Models;
using NotificationApp.Data.Dao;

namespace NotificationApp.Data.Api.EventApiRepo
{
    public interface IEventApiRepo
    {
        public Task<Event> Read(int id, CancellationToken c);
        public Task<List<Event>> List(EventListParamApiDataModel p, CancellationToken c);
    }
}
