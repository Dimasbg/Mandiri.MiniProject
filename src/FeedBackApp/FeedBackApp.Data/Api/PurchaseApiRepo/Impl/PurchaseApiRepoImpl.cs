using FeedBackApp.Data.Api.PurchaseApiRepo.Models;
using FeedBackApp.Data.Dao;
using Mandiri.MiniProject.Utilities.Base;

namespace FeedBackApp.Data.Api.PurchaseApiRepo.Impl
{
    public class PurchaseApiRepoImpl :BaseApiCall, IPurchaseApiRepo
    {
        private readonly IHttpClientFactory _hc;

        public PurchaseApiRepoImpl(IHttpClientFactory hc)
        {
            _hc = hc;
        }
        private static string GenerateEndPoint(string route = "")
            => $"/V1/Purchase{route}";

        public async Task<List<Purchase>> List(PurchaseListParamApiDataModel p, CancellationToken c)
        {
            var cl = _hc.CreateClient("Ticketing.App");

            return await CallEndpoint<List<Purchase>>(cl.GetAsync(GenerateEndPoint($"{p.ToQueryString()}"), c), c);
        }
    }
}
