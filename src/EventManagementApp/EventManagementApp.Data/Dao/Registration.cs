namespace EventManagementApp.Data.Dao
{
    public class Registration
    {
        public int RegistrationId { get; set; }
        public virtual Event Event { get; set; }
        public int UserId { get; set; }
        public DateTime RegistrationDateTime { get; set; }
        public void Update(Registration d)
        {
            Event = d.Event;
            RegistrationDateTime = d.RegistrationDateTime;
            UserId = d.UserId;
            Event = d.Event;
        }
    }
}
