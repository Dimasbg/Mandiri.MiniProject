using EventManagementApp.Data.Dao;
using System.ComponentModel.DataAnnotations;

namespace EventManagementApp.Api.Models.EventModels
{
    public class EventApiModel
    {
        public string? Name { get; set; }
        [DataType(DataType.Text)]
        public string? Description { get; set; }
        public string? Location { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public Event ToDao(int id = 0)
            => new Event
            {
                EventId = id,
                Location = Location,
                Description = Description,
                EndDateTime = EndDateTime,
                Name = Name,
                StartDateTime = StartDateTime
            };
    }
}
