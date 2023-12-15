namespace FeedBackApp.Data.Api.PurchaseApiRepo.Models
{
    public class PurchaseListParamApiDataModel
    {
        public DateTime? PurchaseDateTime { get; set; }
        public int? Quantity { get; set; }
        public int? UserId { get; set; }
        public int? EventId { get; set; }

        public string ToQueryString()
        {
            QueryString result = new QueryString();

            if (Quantity.HasValue)
                result = result.Add(nameof(Quantity), Quantity.Value.ToString());

            if (PurchaseDateTime.HasValue)
                result = result.Add(nameof(PurchaseDateTime), PurchaseDateTime.Value.ToString());

            if (UserId.HasValue)
                result = result.Add(nameof(UserId), UserId.Value.ToString());
            if (EventId.HasValue)
                result = result.Add(nameof(EventId), EventId.Value.ToString());

            return result.ToUriComponent();
        }
    }
}
