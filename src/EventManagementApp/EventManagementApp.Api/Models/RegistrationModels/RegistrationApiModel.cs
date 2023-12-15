using EventManagementApp.Data.Dao;

namespace EventManagementApp.Api.Models.RegistrationModels
{
    public class RegistrationApiModel
    {
        public int EventId { get; set; }
        public int UserId { get; set; }
        public DateTime RegistrationDateTime { get; set; }
        public Registration ToDao(int id = 0)
            => new Registration
            {
                Event = new Event { EventId = EventId },
                RegistrationDateTime = RegistrationDateTime,
                RegistrationId = id,
                UserId = UserId
            };
    }
}
