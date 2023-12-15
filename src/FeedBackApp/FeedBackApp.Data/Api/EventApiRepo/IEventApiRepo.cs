using FeedBackApp.Data.Api.EventApiRepo.Models;
using FeedBackApp.Data.Dao;

namespace FeedBackApp.Data.Api.EventApiRepo
{
    public interface IEventApiRepo
    {
        public Task<Event> Read(int id, CancellationToken c);
        public Task<List<Event>> List(EventListParamApiDataModel p, CancellationToken c);
    }
}
