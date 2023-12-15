using FeedBackApp.Data.Api.PurchaseApiRepo.Models;
using FeedBackApp.Data.Dao;

namespace FeedBackApp.Data.Api.PurchaseApiRepo
{
    public interface IPurchaseApiRepo
    {
        public Task<List<Purchase>> List(PurchaseListParamApiDataModel p, CancellationToken c);
    }
}
