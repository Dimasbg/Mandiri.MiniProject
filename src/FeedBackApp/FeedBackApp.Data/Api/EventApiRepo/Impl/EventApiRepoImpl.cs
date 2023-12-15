using FeedBackApp.Data.Api.EventApiRepo.Models;
using FeedBackApp.Data.Dao;
using Mandiri.MiniProject.Utilities.Base;

namespace FeedBackApp.Data.Api.EventApiRepo.Impl
{
    public class EventApiRepoImpl : BaseApiCall, IEventApiRepo
    {
        private readonly IHttpClientFactory _hc;

        public EventApiRepoImpl(IHttpClientFactory hc)
        {
            _hc = hc;
        }
        private static string GenerateEndPoint(string route = "")
            => $"/V1/Event{route}";

        public async Task<List<Event>> List(EventListParamApiDataModel p, CancellationToken c)
        {
            var cl = _hc.CreateClient("EventManagement.App");

            return await CallEndpoint<List<Event>>(cl.GetAsync(GenerateEndPoint($"{p.ToQueryString()}"), c), c);
        }

        public async Task<Event> Read(int id, CancellationToken c)
        {
            var cl = _hc.CreateClient("EventManagement.App");

            return await CallEndpoint<Event>(cl.GetAsync(GenerateEndPoint($"/{id}"), c), c);
        }
    }
}
