namespace TicketingApp.Data.Api.EventApiRepo.Models
{
    public class EventListParamApiDataModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string ToQueryString()
        {
            QueryString result = new QueryString();

            if (!string.IsNullOrWhiteSpace(Name))
                result = result.Add(nameof(Name), Name);

            if (!string.IsNullOrWhiteSpace(Description))
                result = result.Add(nameof(Description), Description);

            if (!string.IsNullOrWhiteSpace(Location))
                result = result.Add(nameof(Location), Location);

            if (StartDateTime.HasValue)
                result = result.Add(nameof(StartDateTime), StartDateTime.Value.ToString());

            if (EndDateTime.HasValue)
                result = result.Add(nameof(EndDateTime), EndDateTime.Value.ToString());

            return result.ToUriComponent();
        }
    }
}
