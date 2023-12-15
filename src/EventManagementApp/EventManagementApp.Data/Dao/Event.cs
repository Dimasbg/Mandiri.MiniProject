using System.ComponentModel.DataAnnotations;

namespace EventManagementApp.Data.Dao
{
    public class Event
    {
        public int EventId { get; set; }
        public string? Name { get; set; }
        [DataType(DataType.Text)]
        public string? Description { get; set; }
        public string? Location { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public void Update(Event d)
        {
            Name = d.Name;
            Description = d.Description;
            Location = d.Location;
            StartDateTime = d.StartDateTime;
            EndDateTime = d.EndDateTime;
        }
    }
}
